using ConfTool.Client;
using ConfTool.Client.Services;
using ConfTool.Shared.Services;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazor.GrpcWeb.DevTools;
using ConfTool.Client.Features.About.WebCam;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("ConfTool.ServerAPI", client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("ConfTool.ServerAPI"));

builder.Services.AddScoped(services =>
{
    var baseAddressMessageHandler = services.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
    baseAddressMessageHandler.InnerHandler = new HttpClientHandler();
    var backendUrl = builder.HostEnvironment.BaseAddress;

    // Create a channel with a GrpcWebHandler that is addressed to the backend server.
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, baseAddressMessageHandler);

    return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
});

builder.Services.AddGrpcService<IConferencesService>();
builder.Services.EnableGrpcWebDevTools();


builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<WebcamService>();
builder.Services.AddMudServices();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Oidc", options.ProviderOptions);
});

await builder.Build().RunAsync();
