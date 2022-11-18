
using Blazored.LocalStorage;
using ForumApiDataService.Client;
using ForumApiWebLibrary.Client;
using ForumWasmAuthService.Authentication;
using ForumWasmAuthService.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);

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
var forumClientOptions = builder.Configuration.GetSection(nameof(ForumApiClientOptions)).Get<ForumApiClientOptions>();
builder.Services.AddForumIdentityClient(options =>
{
    options.Application = forumClientOptions.Application;
    options.ServiceUri = forumClientOptions.ServiceUri;
    options.DbSchema = forumClientOptions.DbSchema;
}, httpClient =>
{
    var headers = httpClient.DefaultRequestHeaders;
    headers.Add(nameof(forumClientOptions.Application), forumClientOptions.Application);
    httpClient.BaseAddress = new Uri(forumClientOptions.ServiceUri);
}).AddForumAuthenticationHeader();


builder.Services.AddAuthorizationCore();
builder.Services.AddTelerikBlazor();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddTelerikBlazor();

await builder.Build().RunAsync();

