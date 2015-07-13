using Chimote.Tridion.Templating.Intranet.Common;

using Tridion.ContentManager.CommunicationManagement;

namespace Chimote.Tridion.Templating.Intranet.Views
{
    public abstract class DynamicComponentView : ComponentView
    {
        /// <summary>
        /// Override this method to share Context.PageScope data with a PT and all subsequent
        /// CTs on the same page. This method is call by a PT rendition prior to the call to
        /// Context.Engine.RenderComponentPresentation(TcmUri componentUri, TcmUri componentTemplateUri).
        /// </summary>
        /// <param name="presentation">ComponentPresentation object.</param>
        public virtual void PopulatePageData(ComponentPresentation presentation)
        {
            Guard.Requires(this.Context.IsPageContext);
        }

        protected override void CheckPreconditions()
        {
            Guard.Requires(this.Context.IsDynamicTemplate(this.Context.Template));
        }
    }
}