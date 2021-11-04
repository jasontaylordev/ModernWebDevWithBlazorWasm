using BlazorDemo.Client;
using BlazorDemo.Client.Services;
using BlazorDemo.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    }.EnableIntercept(sp));

builder.Services.AddSingleton<IJSInProcessRuntime>(services =>
    (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

builder.Services.AddLoadingBar();

builder.Services.AddValidatorsFromAssemblyContaining<TodoListValidator>();

builder.Services.AddScoped<SessionStorageService>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IWeatherClient>()
    .AddClasses().AsImplementedInterfaces()
    .WithScopedLifetime());

builder.UseLoadingBar();

await builder.Build().RunAsync();
