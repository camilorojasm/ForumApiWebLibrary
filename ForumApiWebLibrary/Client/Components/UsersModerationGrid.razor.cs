using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class UsersModerationGrid : ComponentBase
    {
        public UsersModerationModel UsersModeration { get; set; } = new UsersModerationModel();

        private TelerikGrid<UserModerationModel> GridRef { get; set; }

        //private async Task FetchData()
        //{
        //    UsersModeration = await ForumApiClient.GetUsersModerationAsync();
        //}
        private async Task OnCheckBoxValueChangedAsync(bool value, UserModerationModel userModModel)
        {
            //var item = Deliveries.Where(x => x.ProductName == productName).First();

            // update the model value because the framework does not allow two-way binding when the event is used
            if (value)
            {
                var response = await ForumApiClient.PostAccountModBanAsync(userModModel);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            GridRef?.Rebind();
                            //TopicsModelListViewRef.Rebind();
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation(res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        await ForumAuthService.Logout();
                        NavigationManager.NavigateTo("/login");
                        break;
                    default:
                        return;
                }

            }

        }

        private async Task ReadItems(GridReadEventArgs args)
        {
            UsersModeration = await ForumApiClient.GetUsersModerationAsync();
            //GridData = UsersModeration.UsersModerationList;

            args.Data = UsersModeration.UsersModerationList;
            args.Total = UsersModeration.UsersModerationList.Count;
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    UsersModeration = await ForumApiClient.GetUsersModerationAsync();
        //}
    }
}
