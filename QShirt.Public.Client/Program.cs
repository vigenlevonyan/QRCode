using Blazored.LocalStorage;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using QShirt.Public.Client;
using QShirt.Public.Client.Components;
using QShirt.Public.Client.InteropServices;
using QShirt.Public.Proxy.Generated.ServiceProxies;
using QShirt.Public.Proxy.ServiceProxies;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// session + authorization
builder.Services.AddScoped<Cookies>();
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<UserSession>());
builder.Services.AddAuthorizationCore();

// Telerik components
builder.Services.AddTelerikBlazor();
//builder.Services.AddSingleton(typeof(ITelerikStringLocalizer), typeof(TelerikLocalizer));

// local storage
builder.Services.AddBlazoredLocalStorage();

// proxies for commands/queries
builder.Services.AddServiceProxies();

// messages
builder.Services.AddSingleton<QShirt.Public.Client.InteropServices.Messages>();
builder.Services.AddScoped<Blocker>();

builder.Services.AddScoped<Downloader>();

// executor!
builder.Services
    .AddScoped<IExecutor>(provider =>
        new QShirt.Public.Client.Executor(provider.GetRequiredService<QShirt.Public.Grpc.Executor.ExecutorClient>(),
            provider.GetRequiredService<UserSession>()));

// grpc
builder.Services
    .AddGrpcClient<QShirt.Public.Grpc.Executor.ExecutorClient>((_, options) =>
    {
        options.Address = new Uri(builder.HostEnvironment.BaseAddress);
        options.ChannelOptionsActions.Add(o => { o.MaxReceiveMessageSize = 400 * 1024 * 1024; });
    })
    .ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Automapper to services
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

await SetCultureAsync(app);

await app.RunAsync();

static async Task SetCultureAsync(WebAssemblyHost host)
{
    var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
    var cultureName = await jsRuntime.InvokeAsync<string>("blazorCulture.get");

    if (cultureName != null)
    {
        var culture = new CultureInfo(cultureName);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
