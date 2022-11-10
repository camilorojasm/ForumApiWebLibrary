using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

        public int PageSize { get; set; } = 10;

        //protected override async Task OnInitializedAsync()
        //{
        //    await FetchData();

        //    await base.OnInitializedAsync();
        //}

        //private async Task FetchData(int pageNumber, int pageSize)
        //{
        //    switch (ActiveTabIndex)
        //    {
        //        case 0:
        //            TopicsModel = await ForumApiClient.GetTopicsActiveViewAsync(FId, 1, PageSize);
        //            break;
        //        case 1:
        //            TopicsModel = await ForumApiClient.GetTopicsRecentViewAsync(FId, 1, PageSize);
        //            break;
        //        case 2:
        //            TopicsModel = await ForumApiClient.GetTopicsUpCountViewAsync(FId, 1, PageSize);
        //            break;
        //    }
        //}

        //async Task PageChangedHandler(ListViewCommandEventArgs args)
        //{
        //    //result = $"The user is now on page {currPageIndex}";
        //    var x = "String";
        //}
        //async Task PageChangedHandler(int currPageIndex)
        //{
        //    //result = $"The user is now on page {currPageIndex}";
        //}

        async Task OnReadHandler(ListViewReadEventArgs args)
        {
            switch (ActiveTabIndex)
            {
                case 0:
                    TopicsModel = await ForumApiClient.GetTopicsActiveViewAsync(FId, args.Request.Page, args.Request.PageSize);
                    break;
                case 1:
                    TopicsModel = await ForumApiClient.GetTopicsRecentViewAsync(FId, args.Request.Page, args.Request.PageSize);
                    break;
                case 2:
                    TopicsModel = await ForumApiClient.GetTopicsUpCountViewAsync(FId, args.Request.Page, args.Request.PageSize);
                    break;
            }

            args.Data = TopicsModel.TopicsSortedByRanking;
            args.Total = TopicsModel.TotalRowCount;
            //args.Data = await FetchData(args.Request.Page, args.Request.PageSize);
            //args.Total = await GetTotalItemsCount();
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
                    //do something with the error
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }

            //await OnReadHandler();
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
