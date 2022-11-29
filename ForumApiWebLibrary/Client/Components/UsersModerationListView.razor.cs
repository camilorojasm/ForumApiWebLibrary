using ForumApiDataService.Client;
using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class UsersModerationListView : ComponentBase
    {
        public UsersModerationModel UsersModeration { get; set; } = new UsersModerationModel();

        protected override async Task OnInitializedAsync()
        {
            UsersModeration = await ForumApiClient.GetUsersModerationAsync();
        }

        async Task OnBan(ListViewCommandEventArgs args)
        {

        }

        async Task OnUnban(ListViewCommandEventArgs args)
        {

        }
    }
}
