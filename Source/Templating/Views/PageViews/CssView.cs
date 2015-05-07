using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.PageViews
{
    public class CssView : PageView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Css; }
        }

        protected override string Render()
        {
            return this.RenderPresentations();
        }
    }
}