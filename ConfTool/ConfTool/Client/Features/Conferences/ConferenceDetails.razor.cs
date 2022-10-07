using ConfTool.Client.Features.Conferences.Utils;
using ConfTool.Client.Services;
using ConfTool.Shared.DTO;
using ConfTool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ConfTool.Client.Features.Conferences
{
    public partial class ConferenceDetails
    {
        [Inject] private HttpClient httpClient { get; set; } = default!;
        [Inject] private IConferencesService conferencesService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private DialogService dialogService { get; set; } = default!;
        [Parameter] public Guid? Id { get; set; }
        [Parameter] public string Mode { get; set; }

        private ConferenceDetail? conf;
        private bool onlyShow => Mode == ConferenceMode.Show;

        protected override async Task OnInitializedAsync()
        {
            if (Mode == ConferenceMode.Add)
            {
                conf = new ConferenceDetail();
                conf.ID = Guid.NewGuid();
            }
            else if (Id != null && Id != Guid.Empty)
            {
                try
                {
                    var result = await conferencesService.GetConferenceByIdAsync(new ConferenceRequestModel { Id = Id ?? Guid.Empty });
                    if(!result.Successfull)
                    {
                        await dialogService.AlertAsync($"Beim abrufen der Konferenz ist was schief gelaufen. Error: {result.Error}");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            await base.OnInitializedAsync();
        }

        private async Task OnSubmit()
        {
            var confirm = await dialogService.ConfirmAsync("Wollen Sie wirklich speichern?");
            if (confirm)
            {
                // Call api POST/PUT
                if (Mode == ConferenceMode.Edit)
                {
                    await conferencesService.UpdateConferenceAsync(conf);
                    navigationManager.NavigateTo("/conferences");
                }
                else if (Mode == ConferenceMode.Add)
                {
                    await conferencesService.AddNewConferenceAsync(conf);
                    navigationManager.NavigateTo("/conferences");
                }
            }
        }

        private void GoBack()
        {
            navigationManager.NavigateTo("/conferences");
        }
    }
}