using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class ForumGroupListView : ComponentBase
    {
        [Parameter]
        public long ForumGroupId { get; set; } = 0;

    }
}
