using Microsoft.AspNetCore.Components;
using System;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Index
    {
        MarkupString helloString;

        void SayHelloHandler()
        {
            string msg = string.Format("Hello from <strong>Telerik Blazor</strong> at {0}.<br /> Now you can use C# to write front-end!", DateTime.Now);
            helloString = new MarkupString(msg);
        }

    }
}
