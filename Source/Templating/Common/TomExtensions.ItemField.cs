using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static partial class TomExtensions
    {
        public static bool HasValue(this ItemField field)
        {
            if (field == null)
            {
                return false;
            }

            var textField = field as TextField;

            if (textField != null)
            {
                return TridionExtensions.HasValue(textField);
            }

            var componentLinkField = field as ComponentLinkField;

            if (componentLinkField != null)
            {
                return TridionExtensions.HasValue(componentLinkField);
            }

            var dateField = field as DateField;

            if (dateField != null)
            {
                return TridionExtensions.HasValue(dateField);
            }

            var embeddedSchemaField = field as EmbeddedSchemaField;

            if (embeddedSchemaField != null)
            {
                return TridionExtensions.HasValue(embeddedSchemaField);
            }

            var keywordField = field as KeywordField;

            if (keywordField != null)
            {
                return TridionExtensions.HasValue(keywordField);
            }

            var numberField = field as NumberField;

            if (numberField != null)
            {
                return TridionExtensions.HasValue(numberField);
            }

            return false;
        }
    }
}