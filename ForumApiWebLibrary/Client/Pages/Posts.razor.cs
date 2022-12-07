using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Posts : ComponentBase
    {
        [Parameter]
        public long TId { get; set; }

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
