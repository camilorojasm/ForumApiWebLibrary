using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class TopicsByRankingListView : ComponentBase
    {
        [CascadingParameter]
        public long FId { get; set; }

        [CascadingParameter]
        public int ActiveTabIndex { get; set; }

        public TopicsModel TopicsModel { get; set; } = new TopicsModel();

        public TelerikListView<TopicModel> TopicsModelListViewRef { get; set; }

        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    await FetchData();

        //    await base.OnInitializedAsync();
        //}

        private async Task FetchData(int pageNumber, int pageSize)
        {
            switch (ActiveTabIndex)
            {
                case 0:
                    TopicsModel = await ForumApiClient.GetTopicsActiveViewAsync(FId, pageNumber, pageSize);
                    break;
                case 1:
                    TopicsModel = await ForumApiClient.GetTopicsRecentViewAsync(FId, pageNumber, pageSize);
                    break;
                case 2:
                    TopicsModel = await ForumApiClient.GetTopicsUpCountViewAsync(FId, pageNumber, pageSize);
                    break;
            }
        }

        async Task PageChangedHandler(int currPageIndex)
        {
            //result = $"The user is now on page {currPageIndex}";
        }

        async Task OnReadHandler(ListViewReadEventArgs args)
        {
            PageIndex = args.Request.Page;

            await FetchData(args.Request.Page, args.Request.PageSize);

            args.Data = TopicsModel.TopicsSortedByRanking;
            args.Total = TopicsModel.TotalRowCount;
        }

        async Task OnFlag(ListViewCommandEventArgs args)
        {
        }

        async Task OnVoteUp(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;
            var index = TopicsModel.TopicsList.FindIndex(itm => itm.TId == item.TId);

            if (item.TvId == null && index>-1)
            {
                var response = await ForumApiClient.PostTopicVoteAsync(item, 1);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            TopicsModelListViewRef.Rebind();
                        }
                        //update memory values
                        //TopicsModel.TopicsList[index].TvId = reply.TvId;
                        //TopicsModel.TopicsList[index].Vote = 1;
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation(res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        {
                            await ForumAuthService.Logout();
                            NavigationManager.NavigateTo("/login");
                        }
                        break;
                    default:
                        return;
                }
            }
            else
            {
                if (item.Vote != 1)
                {
                    var response = await ForumApiClient.PutTopicVoteAsync(item, 1);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            {
                                TopicsModelListViewRef.Rebind();
                            }
                            break;
                        case HttpStatusCode.BadRequest:
                            {
                                string res = await response.Content.ReadAsStringAsync();
                                Logger.LogInformation(res);
                            }
                            break;
                        case HttpStatusCode.Unauthorized:
                            {
                                await ForumAuthService.Logout();
                                NavigationManager.NavigateTo("/login");
                            }
                            break;
                        default:
                            return;

                    }
                }
            }
        }

        async Task OnVoteDown(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;
            var index = TopicsModel.TopicsList.FindIndex(itm => itm.TId == item.TId);

            if (item.TvId == null && index>-1)
            {
                var response = await ForumApiClient.PostTopicVoteAsync(item, 0);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            TopicsModelListViewRef.Rebind();
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation("Here " + res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        {
                            await ForumAuthService.Logout();
                            NavigationManager.NavigateTo("/login");
                        }
                        break;
                    default:
                        return;
                }
            }
            else
            {
                if (item.Vote != 0)
                {
                    var response = await ForumApiClient.PutTopicVoteAsync(item, 0);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            {
                                //var msg = await response.Content.ReadAsStringAsync();
                                //Logger.LogInformation(msg);
                                TopicsModelListViewRef.Rebind();
                            }
                            break;
                        case HttpStatusCode.BadRequest:
                            {
                                string res = await response.Content.ReadAsStringAsync();
                                Logger.LogInformation(res);
                            }
                            break;
                        case HttpStatusCode.Unauthorized:
                            {
                                await ForumAuthService.Logout();
                                NavigationManager.NavigateTo("/login");
                            }
                            break;
                        default:
                            return;

                    }
                }
            }
        }


        async Task OnVoteDelete(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;
            var index = TopicsModel.TopicsList.FindIndex(itm => itm.TId == item.TId);

            if (item.TvId != null && index>-1)
            {
                var response = await ForumApiClient.DeleteTopicVoteAsync(item);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        {
                            TopicsModelListViewRef.Rebind();
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation(res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        {
                            await ForumAuthService.Logout();
                            NavigationManager.NavigateTo("/login");
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        async Task ReplyHandler(ListViewCommandEventArgs args)
        {
            //result = $"The user is now on page {currPageIndex}";
            NavigationManager.NavigateTo("/verify");
        }

        async Task UpdateHandler(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;

            var response = await ForumApiClient.PutTopicAsync(item);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.BadRequest:
                    string res = await response.Content.ReadAsStringAsync();
                    Logger.LogInformation(res);
                    //do something with the error
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }

            //// update the local view-model data with the service data
            //await GetListViewData();
            int index = TopicsModel.TopicsSortedByRanking.FindIndex(itm => itm.TId == item.TId);
            if (index > -1)
            {
                TopicsModel.TopicsList[index] = item;
            }

        }

        async Task DeleteHandler(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;

            //await GetListViewData();
        }

        async Task CreateHandler(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;

            var response = await ForumApiClient.PostTopicAsync(item);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    break;
                case HttpStatusCode.BadRequest:
                    string res = await response.Content.ReadAsStringAsync();
                    Logger.LogInformation(res);
                    //do something with the error
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }
        }

        async Task EditHandler(ListViewCommandEventArgs e)
        {
            TopicModel currItem = e.Item as TopicModel;

            // prevent opening an item for editing on condition
            //if (currItem.Id < 2)
            //{
            //    e.IsCancelled = true;
            //}
        }

        public TopicModel OnModelInitHandler()
        {
            return new TopicModel(FId, String.Empty, String.Empty);
        }

    }
}
