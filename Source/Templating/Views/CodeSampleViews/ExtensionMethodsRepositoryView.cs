using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    public class ExtensionMethodsRepositoryView : View<Component>
    {
        protected override string Render()
        {
            /* *******************************************************************************
             * Repository is the base type of the following types:
             * - Publication
             * - BluePrintNode
             ******************************************************************************* */

            /* *******************************************************************************
             * Repository.GetMetadataFields()
             * ***************************************************************************** */

            ItemFields metadata = this.Context.Publication.GetMetadataFields();

            /* *******************************************************************************
             * Repository.GetMetadataField<T>(string fieldName) where T : ItemField
             * ***************************************************************************** */

            TextField textField = this.Context.Publication.GetMetadataField<TextField>("environment");

            return string.Empty;
        }
    }
}