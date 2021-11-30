using Eagle.Framework.Client.Manager;
using Eagle.Framework.Client.Navigation;
using Eagle.Framework.Client.UI;

namespace VisionOnTheWeb
{
    public class WebPageManager : BasePageManager
    {
        public WebPageManager(ClientManager clientManager) : base(clientManager)
        {
        }

        public override string Framework { get; }
        public override IPage CurrentPage { get; set; }

        public override Task<string> AskDeleteForPageAsync(PageContext pageContent, string title, string question, string accept, string cancel)
        {
            return Task.FromResult(accept);
        }

        public override void CopyToClipboard(string text)
        {
        }

        public override IPage GetCurrentPage()
        {
            return null;
        }

        public override List<IPage> GetPages()
        {
            return new List<IPage>();
        }

        public override Task RunOnUIThread(Action action)
        {
            action();
            return Task.CompletedTask;
        }

        public override Task RunOnUIThread(Func<Task> action)
        {
            return action();
        }

        public override void SetCurrentPage(IPage page)
        {
        }

        protected override Task HandleClientActionInternal(string uri)
        {
            return Task.CompletedTask;
        }

        protected override void OpenPageInternal(IPage afterPage, IPage page, PageIdentifier pageIdentifier)
        {
            throw new NotImplementedException();
        }

        protected override IPage ShowPage(IPage afterPage, IPage page, PageIdentifier newPageRequest)
        {
            return page;
        }
    }
}
