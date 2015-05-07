using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.PageViews
{
    public class JsView : PageView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Js; }
        }

        protected override string Render()
        {
            return this.RenderPresentations();
        }
    }
}