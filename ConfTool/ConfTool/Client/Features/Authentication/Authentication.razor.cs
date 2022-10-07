using Microsoft.AspNetCore.Components;

namespace ConfTool.Client.Features.Authentication
{
    public partial class Authentication
    {
        [Inject] private NavigationManager _navigationManager { get; set; }

        [Parameter]
        public string Action { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            Console.WriteLine(Action);
            if (Action == "logged-out")
            {
                _navigationManager.NavigateTo("/", replace: true);
            }
            base.OnParametersSet();
        }
    }
}