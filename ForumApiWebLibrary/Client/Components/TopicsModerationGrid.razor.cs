using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class TopicsModerationGrid : ComponentBase
    {
        public TopicsModerationModel TopicsModeration { get; set; } = new TopicsModerationModel();

        private TelerikGrid<TopicModerationModel> GridRef { get; set; }

        //private async Task FetchData()
        //{
        //    TopicsModeration = await ForumApiClient.GetTopicsModerationAsync();
        //}
        private async Task OnCheckBoxValueChangedAsync(bool value, TopicModerationModel TopicModModel)
        {
            HttpResponseMessage response;

            // update the model value because the framework does not allow two-way binding when the event is used
            if (value)
            {
                response = await ForumApiClient.PostTopicModBanAsync(TopicModModel);

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
                response = await ForumApiClient.DeleteTopicModBanAsync(TopicModModel);

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
            TopicsModeration = await ForumApiClient.GetTopicsModerationAsync();
            //GridData = TopicsModeration.TopicsModerationList;

            args.Data = TopicsModeration.TopicsModerationList;
            args.Total = TopicsModeration.TopicsModerationList.Count;
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    TopicsModeration = await ForumApiClient.GetTopicsModerationAsync();
        //}
    }
}
