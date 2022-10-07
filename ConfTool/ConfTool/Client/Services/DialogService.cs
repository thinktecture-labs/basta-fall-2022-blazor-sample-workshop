using Microsoft.JSInterop;
using MudBlazor;

namespace ConfTool.Client.Services
{
    public class DialogService : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        public DialogService(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "/js/dialog.js").AsTask());
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<bool>("confirm", message);
        }

        public async Task AlertAsync(string message)
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("alert", message);
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }

}
