using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.PageViews
{
    public class HtmlView : PageView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Html; }
        }

        protected override string Render()
        {
            return this.RenderPresentations();
        }
    }
}