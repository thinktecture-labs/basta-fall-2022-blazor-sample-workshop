using Microsoft.AspNetCore.Components;

namespace ConfTool.Client.Features.About.WebCam
{
    public partial class WebCam
    {
        [Inject] private WebcamService _webcamService { get; set; } = default!;

        private readonly WebcamOptions _options = new WebcamOptions()
        { CanvasId = "canvas", VideoId = "video", PhotoId = "photo" };
        protected override void OnInitialized()
        {
            _options.Width = 320;
        }

        private async Task StartVideoAsync()
        {
            await _webcamService.StartVideoAsync(_options);
        }

        private async Task TakePictureAsync()
        {
            await _webcamService.TakePictureAsync();
        }
    }
}