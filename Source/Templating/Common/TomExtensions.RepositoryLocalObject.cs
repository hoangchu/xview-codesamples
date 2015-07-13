using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static partial class TomExtensions
    {
        public static Component GetMetadataComponentRecursive(this RepositoryLocalObject repositoryLocalObject, string fieldPath)
        {
            var itemField = GetMetadataFieldRecursive<ComponentLinkField>(repositoryLocalObject, fieldPath);

            return itemField != null ? itemField.Value : null;
        }

        public static string GetMetadataTextRecursive(this RepositoryLocalObject repositoryLocalObject, string fieldPath)
        {
            var itemField = GetMetadataFieldRecursive<TextField>(repositoryLocalObject, fieldPath);

            return itemField != null ? itemField.Value : null;
        }

        /// <summary>
        /// Gets metadata field recursively for the given fieldName.
        /// </summary>
        /// <typeparam name="T">ItemField or a derived type.</typeparam>
        /// <param name="repositoryLocalObject">RepositoryLocalObject object.</param>
        /// <param name="fieldPath">Metadata field name.</param>
        /// <returns>Object of the given type.</returns>
        public static T GetMetadataFieldRecursive<T>(this RepositoryLocalObject repositoryLocalObject, string fieldPath)
            where T : ItemField
        {
            T itemField = null;

            while (true)
            {
                var metadata = repositoryLocalObject.GetMetadataFields();

                if (metadata != null)
                {
                    itemField = metadata.GetField<T>(fieldPath);
                }

                if (!itemField.HasValue())
                {
                    if (
                        !(repositoryLocalObject is OrganizationalItem &&
                          ((OrganizationalItem)repositoryLocalObject).IsRootOrganizationalItem))
                    {
                        repositoryLocalObject = repositoryLocalObject.OrganizationalItem;
                        continue;
                    }
                }

                break;
            }

            return itemField;
        }
    }
}