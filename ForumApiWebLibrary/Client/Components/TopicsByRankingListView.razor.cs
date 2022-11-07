using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //[CascadingParameter]
        public TopicsModel TopicsModel { get; set; } = new TopicsModel();

        public int PageSize { get; set; } = 10;

        //public TopicsModel TopicsModel { get; set; } = new TopicsModel();

        protected override async Task OnInitializedAsync()
        {
            switch (ActiveTabIndex)
            {
                case 0:
                    TopicsModel = await ForumApiClient.GetTopicsActiveViewAsync(FId, 1, PageSize);
                    break;
                case 1:
                    TopicsModel = await ForumApiClient.GetTopicsRecentViewAsync(FId, 1, PageSize);
                    break;
                case 2:
                    TopicsModel = await ForumApiClient.GetTopicsUpCountViewAsync(FId, 1, PageSize);
                    break;
            }

            await base.OnInitializedAsync();
        }

        async Task PageChangedHandler(int currPageIndex)
        {
            //result = $"The user is now on page {currPageIndex}";
        }

        async Task UpdateHandler(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;

            //// perform actual data source operation here through your service
            //await MyService.Update(item);

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

            // perform actual data source operation here through your service
            //await MyService.Delete(item);

            //// update the local view-model data with the service data
            //await GetListViewData();
        }

        async Task CreateHandler(ListViewCommandEventArgs args)
        {
            TopicModel item = (TopicModel)args.Item;

            TopicsModel.TopicsList.Insert(0, item);

            // perform actual data source operation here through your service
            //await MyService.Create(item);

            //// update the local view-model data with the service data
            //await GetListViewData();
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
            return new TopicModel(0, String.Empty, String.Empty);
        }

    }
}
