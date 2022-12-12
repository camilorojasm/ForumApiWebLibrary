using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Profile : ComponentBase
    {
        public UserProfileModel ProfileModel { get; set; } = new UserProfileModel();

        List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };

        int MaxFileSize { get; set; } = 1024 * 1024; // 1 MB

        async Task OnSelectHandler(FileSelectEventArgs args)
        {
            foreach (var file in args.Files)
            {
                var buffer = new byte[file.Stream.Length];
                await file.Stream.ReadAsync(buffer);
            }
        }
        private async Task FetchData()
        {
            ProfileModel = await ForumApiClient.GetUserProfileViewAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchData();
            await base.OnInitializedAsync();
        }

        public bool ValidSubmit { get; set; } = false;

        async void HandleValidSubmit()
        {
            ValidSubmit = true;

            var userEntry = new UserEntryModel()
            {
                UserName = ProfileModel.UserName,
                EmailAddress = ProfileModel.EmailAddress,
                OptinInternal = ProfileModel.OptinInternal,
                OptinExternal = ProfileModel.OptinExternal
            };

            var response = await ForumApiClient.PutAccountAsync(userEntry);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    await FetchData();
                    break;
                case HttpStatusCode.BadRequest:
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        Logger.LogInformation(res);
                    }
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }

            ValidSubmit = false;

            StateHasChanged();
        }

        void HandleInvalidSubmit()
        {
            ValidSubmit = false;
        }

    }
}
