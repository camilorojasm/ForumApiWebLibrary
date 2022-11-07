using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Topics : ComponentBase
    {

        [Parameter]
        public long FId { get; set; }

        //public TopicsModel TopicsModel { get; set; } = new TopicsModel();

        //public int PageSize { get; set; } = 10;

        public int ActiveTabIndex { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        async Task TabChangedHandler(int newIndex)
        {
            ActiveTabIndex = newIndex;

            //switch (ActiveTabIndex)
            //{
            //    case 0:
            //        TopicsModel = await ForumApiClient.GetTopicsActiveViewAsync(FId, 1, PageSize);
            //        break;
            //    case 1:
            //        TopicsModel = await ForumApiClient.GetTopicsRecentViewAsync(FId, 1, PageSize);
            //        break;
            //    case 2:
            //        TopicsModel = await ForumApiClient.GetTopicsUpCountViewAsync(FId, 1, PageSize);
            //        break;
            //}

        }
    }
}
