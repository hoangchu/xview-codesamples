using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.PageViews
{
    public class XmlView : PageView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Xml; }
        }

        protected override string Render()
        {
            return this.RenderPresentations();
        } 
    }
}