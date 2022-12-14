using Microsoft.AspNetCore.Components;
using System;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Index
    {
        MarkupString demoString;

        void DemoHandler()
        {
            string msg = string.Format("Forum API demonstration web application using <strong>Telerik Blazor</strong> at {0}.<br /> components", DateTime.Now);
            demoString = new MarkupString(msg);
        }

    }
}
