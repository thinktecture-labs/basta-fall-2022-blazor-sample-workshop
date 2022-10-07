using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ConfTool.Shared.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using ConfTool.Shared.Utils;
using ConfTool.Client.Services;
using ConfTool.Shared.Services;

namespace ConfTool.Client.Features.Conferences
{
    public partial class ConferencesOverview
    {
        [Inject] private HttpClient httpClient { get; set; } = default!;
        [Inject] private DialogService dialogService { get; set; } = default!;
        [Inject] private IConferencesService conferencesService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;

        private bool isLoading = false;
        private List<ConferenceOverview>? conferenceOverviews;
        private HubConnection _hubConnection;
        private Guid newAddedId;


        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/conferenceshub"))
            .Build();

            _hubConnection.On(SignalRMethodNames.AddedConference, async (Guid id) =>
            {
                Console.WriteLine("###SignalR - NEW conference added!");
                conferenceOverviews = await httpClient.GetFromJsonAsync<List<ConferenceOverview>>("/api/conferences");
                newAddedId = id;

                await dialogService.AlertAsync($"Eine neue Konferenz wurde hinzugefühgt");
                StateHasChanged();
            });

            await _hubConnection.StartAsync();


            conferenceOverviews = await conferencesService.GetConferencesAsync();
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private void OpenDetails(string mode, Guid? id = null)
        {
            if (id == null)
            {
                navigationManager.NavigateTo($"/conferences/{mode}");
            }
            else
            {
                navigationManager.NavigateTo($"/conferences/{mode}/{id}");
            }
        }
    }
}