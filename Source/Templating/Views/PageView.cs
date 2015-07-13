using System.Text;

using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views
{
    /// <summary>
    /// Represents the base View to render Page model.
    /// In this View define Page related properties and methods to be re-used
    /// in derived Views.
    /// </summary>
    public abstract class PageView : View<Page>
    {
        private ItemFields metadata;

        /// <summary>
        /// Gets the metadata ItemFields of the Page (Model).
        /// </summary>
        protected ItemFields Metadata
        {
            get { return this.metadata ?? (this.metadata = this.Model.GetMetadataFields()); }
        }

        protected override void InitializeRender()
        {
            base.InitializeRender();
            this.metadata = null;
        }

        /// <summary>
        /// Renders output of all component presentations on the Page (Model).
        /// </summary>
        /// <returns>Presentation output string.</returns>
        protected virtual string RenderPresentations()
        {
            var sb = new StringBuilder();

            foreach (var presentation in this.Model.ComponentPresentations)
            {
                sb.Append(this.Context.Engine.RenderComponentPresentation(presentation.Component.Id,
                    presentation.ComponentTemplate.Id));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Shortcut to Context.Engine.RenderComponentPresentation(TcmUri componentUri, TcmUri componentTemplateUri).
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="template">ComponentTemplate.</param>
        /// <returns>Output string.</returns>
        protected virtual string RenderPresentation(Component component, ComponentTemplate template)
        {
            return this.Context.Engine.RenderComponentPresentation(component.Id, template.Id);
        }
    }
}