
using Blazored.LocalStorage;
using ForumApiDataService.Client;
using ForumApiWebLibrary.Client;
using ForumWasmAuthService.Authentication;
using ForumWasmAuthService.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configure AUTHENTICATION
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddForumAuthenticationStateProvider();

var identityClientOptions = builder.Configuration.GetSection(nameof(ForumIdentityClientOptions)).Get<ForumIdentityClientOptions>();
builder.Services.AddForumIdentityClient(httpClient =>
{
    var headers = httpClient.DefaultRequestHeaders;
    headers.Add(nameof(identityClientOptions.Application), identityClientOptions.Application);

    httpClient.BaseAddress = new Uri(identityClientOptions.ServiceUri);
}).AddForumAuthenticationHeader();

builder.Services.AddForumAuthenticationService();


// Configure FORUM API
var apiClientOptions = builder.Configuration.GetSection(nameof(ForumApiClientOptions)).Get<ForumApiClientOptions>();

builder.Services.AddForumIdentityClient(options =>
{
    options.Application = apiClientOptions.Application;
    options.ServiceUri = apiClientOptions.ServiceUri;
    options.DbSchema = apiClientOptions.DbSchema;
}, httpClient =>
{
    var headers = httpClient.DefaultRequestHeaders;
    headers.Add(nameof(apiClientOptions.Application), apiClientOptions.Application);

    httpClient.BaseAddress = new Uri(apiClientOptions.ServiceUri);
}).AddForumAuthenticationHeader();


builder.Services.AddAuthorizationCore();
builder.Services.AddTelerikBlazor();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddTelerikBlazor();

await builder.Build().RunAsync();

