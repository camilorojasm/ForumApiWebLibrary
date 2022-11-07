using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class ForumGroupsListView : ComponentBase
    {
        //[Parameter]
        public ForumGroupsModel ForumGroupsModel { get; set; } = new ForumGroupsModel();
        //public IEnumerable<ForumModel> ListViewData { get; set; }


        protected override async Task OnInitializedAsync()
        {
            ForumGroupsModel = await ForumApiClient.GetForumsViewAsync();
        }

        private void NaviToActive(ListViewCommandEventArgs args)
        {
            NavigationManager.NavigateTo("/");
        }
        
        private void NaviToRecent(ListViewCommandEventArgs args)
        {
            NavigationManager.NavigateTo("/");
        }

        void SelectItem(long itemId)
        {
            ForumGroupModel currItem = ForumGroupsModel.ForumGroupsSortedByDisplayOrder.Where(itm => itm.ForumGroupId == itemId).FirstOrDefault();
            
            if (currItem != null)
            {
                currItem.Selected = !currItem.Selected;
            }
        }
    }
}
