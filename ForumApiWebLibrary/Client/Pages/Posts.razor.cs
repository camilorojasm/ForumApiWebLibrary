using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Posts : ComponentBase
    {
        [Parameter]
        public long TId { get; set; }

    }
}
