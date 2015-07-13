using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.ComponentViews
{
    public class RawdataView : ComponentView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Text; }
        }

        protected override string Render()
        {
            return this.Model.GetText("code");
        }
    }
}