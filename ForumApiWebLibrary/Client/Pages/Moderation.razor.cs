using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Moderation : ComponentBase
    {
        public int ActiveTabIndex { get; set; } = 0;

        async Task TabChangedHandler(int newIndex)
        {
            ActiveTabIndex = newIndex;
        }
    }
}
