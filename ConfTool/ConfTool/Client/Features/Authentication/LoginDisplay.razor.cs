using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ConfTool.Client.Features.Authentication
{
    public partial class LoginDisplay
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private SignOutSessionStateManager _signOutSessionStateManager { get; set; } = default!;
        private async Task BeginSignOut(MouseEventArgs args)
        {
            await _signOutSessionStateManager.SetSignOutState();
            _navigationManager.NavigateTo("authentication/logout");
        }
    }
}