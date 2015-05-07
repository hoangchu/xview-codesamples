using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views
{
    /// <summary>
    /// Represents the base View to render Component model.
    /// This base View is optional, but useful in large implementations.
    /// 
    /// In this View define Component related properties and methods to be re-used
    /// in derived Views.
    /// </summary>
    public abstract class ComponentView : View<Component>
    {
        private ItemFields fields;
        private ItemFields metadata;

        protected ItemFields Fields
        {
            get { return this.fields ?? (this.fields = this.Model.GetFields()); }
        }

        protected ItemFields Metadata
        {
            get { return this.metadata ?? (this.metadata = this.Model.GetMetadataFields()); }
        }
    }
}