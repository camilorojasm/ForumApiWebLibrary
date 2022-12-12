using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Telerik.Blazor.Components.Grid;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Register : ComponentBase
    {
        public RegistrationModel SignUpUser { get; set; } = new RegistrationModel();

        public bool ValidSubmit { get; set; } = false;

        async void HandleValidSubmit()
        {
            ValidSubmit = true;

            //Assign claims
            SignUpUser.Claims = new List<ClaimModel> { new ClaimModel() { Claim = "Member" } };

            var response = await ForumApiClient.PostUserRegistrationAsync(SignUpUser);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    NavManager.NavigateTo("/login");
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
