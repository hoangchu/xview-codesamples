using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    public class ExtensionMethodsRepositoryLocalObjectView : View<Component>
    {
        protected override string Render()
        {
            /* *******************************************************************************
             * Repository is the base type of the following types:
             * - TargetGroup
             * - Keyword
             * - OrganizationalItem
             *      - StructureGroup
             *      - Category
             *      - Folder
             *      - VirtualFolder
             * - VersionedItem
             *      - Page
             *      - Template
             *          - ComponentTemplate
             *          - PageTemplate
             *          - TemplateBuildingBlock
             *      - Component
             *      - Schema
             * - ProcessDefinition
             *      - TridionProcessDefinition
             ******************************************************************************* */

            /* *******************************************************************************
             * Repository.GetMetadataFields()
             * ***************************************************************************** */

            ItemFields metadata = this.Context.Publication.GetMetadataFields();

            /* *******************************************************************************
             * Repository.GetMetadataField<T>(string fieldName) where T : ItemField
             * ***************************************************************************** */

            TextField textField = this.Context.Publication.GetMetadataField<TextField>("environment");

            // Etc.

            return string.Empty;
        }
    }
}