using Tridion.ContentManager.ContentManagement;

namespace XViewCodeSamples.Views
{
    public abstract class DynamicComponentView : ComponentView
    {
        /// <summary>
        /// Override this method to share Context.PageScope data with a PT and  all subsequent
        /// CTs on the same page. This method is call by a PT rendition prior to the call to
        /// Context.Engine.RenderComponentPresentation(TcmUri componentUri, TcmUri componentTemplateUri).
        /// </summary>
        /// <param name="component">Component.</param>
        public virtual void SetPageScopeData(Component component)
        {
        }
    }
}