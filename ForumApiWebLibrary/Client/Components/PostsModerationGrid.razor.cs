using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class PostsModerationGrid : ComponentBase
    {
        public PostsModerationModel PostsModeration { get; set; } = new PostsModerationModel();

        private TelerikGrid<PostModerationModel> GridRef { get; set; }

        //private async Task FetchData()
        //{
        //    PostsModeration = await ForumApiClient.GetPostsModerationAsync();
        //}
        private async Task OnCheckBoxValueChangedAsync(bool value, PostModerationModel postModeration)
        {
            HttpResponseMessage response;

            // update the model value because the framework does not allow two-way binding when the event is used
            if (value)
            {
                response = await ForumApiClient.PostPostModBanAsync(postModeration);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        GridRef?.Rebind();
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
            else
            {
                response = await ForumApiClient.DeletePostModBanAsync(postModeration);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        GridRef?.Rebind();
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
            PostsModeration = await ForumApiClient.GetPostsModerationAsync();
            //GridData = PostsModeration.PostsModerationList;

            args.Data = PostsModeration.PostsModerationList;
            args.Total = PostsModeration.PostsModerationList.Count;
        }
    }
}
