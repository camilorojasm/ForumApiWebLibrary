using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Telerik.Blazor.Components.Breadcrumb;
using Telerik.Blazor.Components.Editor;
using Telerik.Blazor.Components;
using Microsoft.Extensions.Logging;
using ForumApiWebLibrary.Client.Pages;

namespace ForumApiWebLibrary.Client.Components
{
    public partial class PostsTreeView : ComponentBase
    {
        [CascadingParameter]
        public long TId { get; set; }

        //public List<BreadcrumbItem> PostsBreandcrumbItems { get; set; }

        public PostsModel PostsModel { get; set; } = new PostsModel();

        public TelerikTreeList<PostModel> PostModelTreeListRef;

        public TelerikEditor Editor { get; set; } = new TelerikEditor();

        public List<IEditorTool> ToolCollection { get; set; } = new List<IEditorTool>()
        {
            new CustomTool("ColorTools"),
            new CustomTool("CleanFormattingTool"),
            new CustomTool("InsertVideo"),
            new CustomTool("InsertHtmlTools")
        };

        private async Task ExecuteBackColor()
        {
            await Editor.ExecuteAsync(new FormatCommandArgs("backColor", "#00ff00"));
        }

        private async Task ExecuteForeColor()
        {
            await Editor.ExecuteAsync(new FormatCommandArgs("foreColor", "#ff0000"));
        }

        private async Task ExecuteCleanFormatting()
        {
            await Editor.ExecuteAsync(new ToolCommandArgs("cleanFormatting"));
        }

        private async Task ExecuteInsertHr()
        {
            await Editor.ExecuteAsync(new HtmlCommandArgs("insertHtml", "<hr/>"));
        }

        private async Task ExecuteInsertLogo()
        {
            await Editor.ExecuteAsync(new ImageCommandArgs("https://demos.telerik.com/blazor-ui/images/component-icons/svg/editor.svg", "editor-logo"));
        }

        private async Task ExecuteInsertVideo()
        {
            await Editor.ExecuteAsync(new HtmlCommandArgs("insertHtml", "<video controls src=\"video/netfuture.mp4\" width=\"400px\"></video>"));
        }

        async Task GetRecentPostsListAsync()
        {
            PostsModel = await ForumApiClient.GetPostsThreadedViewAsync(TId);

            if (PostsModel.TotalRowCount == 0)
            {
                OnModelInitHandler();
            }

        }

        // initial data generation
        protected override async Task OnInitializedAsync()
        {
            await GetRecentPostsListAsync();
            await base.OnInitializedAsync();
        }

        protected override void OnInitialized()
        {
            //PostsBreandcrumbItems = new List<BreadcrumbItem>
            //{
            //    new BreadcrumbItem { Text = "Home", Icon = "home", Url="/" },
            //    new BreadcrumbItem { Text = "Forums", Icon = "layout-side-by-side", Url="/forums" },
            //    new BreadcrumbItem { Text = "Threads", Icon = "group-section", Url="/forum/3" },
            //    new BreadcrumbItem { Text = "Posts", Icon = "group-collection" }
            //};

            foreach (var tool in EditorToolSets.All)
            {
                ToolCollection.Insert(0, tool);
            }
            ToolCollection.AddRange(new List<IEditorTool>(EditorToolSets.All));

            base.OnInitialized();
        }

        async Task UpdateItem(TreeListCommandEventArgs args)
        {
            var item = args.Item as PostModel;

            var response = await ForumApiClient.PutPostAsync(item);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    await GetRecentPostsListAsync();
                    break;
                case HttpStatusCode.BadRequest:
                    string res = await response.Content.ReadAsStringAsync();
                    //do something with the error
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }

            //int index = PostsModel.RecentPostsList.FindIndex(itm => itm.TId == item.TId);
            //if (index > -1)
            //{
            //    PostsModel.RecentPostsList[index] = item;
            //}

            //await GetRecentPostsListAsync();
        }

        async Task CreateItem(TreeListCommandEventArgs args)
        {
            var item = args.Item as PostModel;
            var parentItem = args.ParentItem as PostModel;
            HttpResponseMessage response = new();

            if (item != null && parentItem == null)
            {
                response = await ForumApiClient.PostPostAsync(item);
            }

            if (item != null && parentItem != null)
            {
                response = await ForumApiClient.PostReplyPostAsync(item, parentItem);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    await GetRecentPostsListAsync();
                    break;
                case HttpStatusCode.BadRequest:
                    string res = await response.Content.ReadAsStringAsync();
                    //do something with the error
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }

            //await GetRecentPostsListAsync();
        }

        async Task DeleteItem(TreeListCommandEventArgs args)
        {
            var item = args.Item as PostModel;

            //await MyService.Delete(item);

            await GetRecentPostsListAsync();
        }

        async Task OnCancelHandler(TreeListCommandEventArgs args)
        {
            PostModel empl = args.Item as PostModel;
        }

        public PostModel OnModelInitHandler()
        {
            return new PostModel() { TId = this.TId };
        }

        async Task OnFlagAsInappropriate(TreeListCommandEventArgs args)
        {
            PostModel item = (PostModel)args.Item;
            var response = await ForumApiClient.PostPostInappropAsync(item);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    await GetRecentPostsListAsync();
                    break;
                case HttpStatusCode.BadRequest:
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        Logger.LogInformation(res);
                    }
                    break;
                case HttpStatusCode.Unauthorized:
                    await ForumAuthService.Logout();
                    NavigationManager.NavigateTo("/login");
                    break;
                default:
                    return;
            }
        }

        async Task OnVoteUp(TreeListCommandEventArgs args)
        {
            PostModel item = (PostModel)args.Item;
            var index = PostsModel.RecentPostsList.FindIndex(itm => itm.PId == item.PId);

            if (item.PvId == null && index > -1)
            {
                var response = await ForumApiClient.PostPostVoteAsync(item, 1);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        await GetRecentPostsListAsync();
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation(res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        await ForumAuthService.Logout();
                        NavigationManager.NavigateTo("/login");
                        break;
                    default:
                        return;
                }
            }
            else
            {
                if (item.Vote != 1)
                {
                    var response = await ForumApiClient.PutPostVoteAsync(item, 1);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            await GetRecentPostsListAsync();
                            break;
                        case HttpStatusCode.BadRequest:
                            {
                                string res = await response.Content.ReadAsStringAsync();
                                Logger.LogInformation(res);
                            }
                            break;
                        case HttpStatusCode.Unauthorized:
                            await ForumAuthService.Logout();
                            NavigationManager.NavigateTo("/login");
                            break;
                        default:
                            return;
                    }

                }
            }

            //PostModelTreeListRef.Rebind();

            //set the tv_id and vote in the memory store
            //int index = PostsModel.RecentPostsList.FindIndex(itm => itm.PId == item.PId);
            //if (index > -1)
            //{
            //    PostsModel.RecentPostsList[index] = item;
            //}
        }

        async Task OnVoteDown(TreeListCommandEventArgs args)
        {
            PostModel item = (PostModel)args.Item;
            var index = PostsModel.RecentPostsList.FindIndex(itm => itm.PId == item.PId);

            if (item.PvId == null && index > -1)
            {
                var response = await ForumApiClient.PostPostVoteAsync(item, 0);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        await GetRecentPostsListAsync();
                        break;
                    case HttpStatusCode.BadRequest:
                        {
                            string res = await response.Content.ReadAsStringAsync();
                            Logger.LogInformation(res);
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        await ForumAuthService.Logout();
                        NavigationManager.NavigateTo("/login");
                        break;
                    default:
                        return;
                }
            }
            else
            {
                if (item.Vote != 0)
                {
                    var response = await ForumApiClient.PutPostVoteAsync(item, 0);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            await GetRecentPostsListAsync();
                            break;
                        case HttpStatusCode.BadRequest:
                            {
                                string res = await response.Content.ReadAsStringAsync();
                                Logger.LogInformation(res);
                            }
                            break;
                        case HttpStatusCode.Unauthorized:
                            await ForumAuthService.Logout();
                            NavigationManager.NavigateTo("/login");
                            break;
                        default:
                            return;
                    }

                }
            }
        }

        async Task OnVoteDelete(TreeListCommandEventArgs args)
        {
            PostModel item = (PostModel)args.Item;
            var index = PostsModel.RecentPostsList.FindIndex(itm => itm.PId == item.PId);

            if (item.PvId != null && index > -1)
            {
                var response = await ForumApiClient.DeletePostVoteAsync(item);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        await GetRecentPostsListAsync();
                        break;
                    case HttpStatusCode.BadRequest:
                        string res = await response.Content.ReadAsStringAsync();
                        Logger.LogInformation(res);
                        //do something with the error
                        break;
                    case HttpStatusCode.Unauthorized:
                        await ForumAuthService.Logout();
                        NavigationManager.NavigateTo("/login");
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
