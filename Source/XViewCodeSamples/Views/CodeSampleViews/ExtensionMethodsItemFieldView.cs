using System;

using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace XViewCodeSamples.Views.CodeSampleViews
{
    public class ExtensionMethodsItemFieldView : View<Component>
    {
        protected override string Render()
        {
            // Each ItemField derived type has the .HasValue() extension method
            // that can be used to check whether such field has a value.

            ItemFields fields = this.Model.GetFields();

            /* *******************************************************************************
             * TextField, XhtmlField, SingleLineTextField, MultiLineTextField, ExternalLinkField
             * ***************************************************************************** */

            bool hasValue = fields.GetField<TextField>("name").HasValue();

            // Alternatives.

            hasValue = fields.GetTexts("name").Count > 0;
            hasValue = fields.GetText("name") != null;

            /* *******************************************************************************
             * ComponentLinkField, MultimediaLinkField
             * ***************************************************************************** */

            hasValue = fields.GetField<ComponentLinkField>("link").HasValue();

            // Alternatives.

            hasValue = fields.GetComponents("link").Count > 0;
            hasValue = fields.GetComponent("link") != null;

            /* *******************************************************************************
             * EmbeddedSchemaField.
             * ***************************************************************************** */

            hasValue = fields.GetField<EmbeddedSchemaField>("paragraphs").HasValue();

            // Alternatives.

            hasValue = fields.GetEmbeddedFields("paragraphs").Count > 0;
            hasValue = fields.GetEmbeddedField("paragraphs") != null;

            /* *******************************************************************************
             * Accessing DateField.
             * ***************************************************************************** */

            hasValue = fields.GetField<DateField>("publish_date").HasValue();
            
            // Alternatives.

            hasValue = fields.GetDates("publish_date").Count > 0;
            hasValue = fields.GetDate("publish_date") != DateTime.MinValue;


            /* *******************************************************************************
             * NumberField.
             * ***************************************************************************** */

            hasValue = fields.GetField<NumberField>("age").HasValue();

            // Alternatives.

            hasValue = fields.GetNumbers("age").Count > 0;
            hasValue = fields.GetNumber("age") != double.MinValue;

            /* *******************************************************************************
             * Accessing KeywordField.
             * ***************************************************************************** */

            hasValue = fields.GetField<KeywordField>("tag").HasValue();

            // Alternatives.

            hasValue = fields.GetKeywords("tag").Count > 0;
            hasValue = fields.GetKeyword("tag") != null;

            return string.Empty;
        }
    }
}