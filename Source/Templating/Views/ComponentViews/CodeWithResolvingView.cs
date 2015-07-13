using Chimote.Tridion.Templating.Intranet.Common;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.ComponentViews
{
    public class CodeWithResolvingView : ComponentView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Text; }
        }

        protected override string Render()
        {
            return new ImagePathResolver(this.Context).Resolve(this.Model.GetText("code"), this.Model.OrganizationalItem.WebDavUrl);
        }
    }
}