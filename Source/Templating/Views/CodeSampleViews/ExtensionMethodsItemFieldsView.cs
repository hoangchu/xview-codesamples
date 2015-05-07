using System;
using System.Collections.Generic;

using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    public class ExtensionMethodsItemFieldsView : View<Component>
    {
        protected override string Render()
        {
            // Model is a Component.

            // Get ItemFields from Component. Then access individual fields from the ItemFields object.

            ItemFields fields = this.Model.GetFields();

            // Alternatively, it's possible to access individual fields directly from a
            // Component. This is made possible for cases in which only one field is needed
            // from a Component.

            // In case you need to access more than one fields from a Component you should
            // get the ItemFields (as above) then access the individual fields from the
            // ItemFields object. This will make your code to execute faster.

            string title = this.Model.GetText("title");

            // Below are sample codes for accessing different ItemField types from ItemFields.

            /* *******************************************************************************
             * Accessing TextField, XhtmlField, SingleLineTextField, MultiLineTextField, ExternalLinkField
             * ***************************************************************************** */

            // IMPORTANT NOTE: when a handler for the TridionExtensions.XhtmlResolver delegate
            // is assigned .GetText() will automatically resolve xhtml content from an XhtmlField.
            // See IntranetController how the TridionExtensions.XhtmlResolver is assigned.

            string text = fields.GetText("text");
            IList<string> texts = fields.GetTexts("texts");

            text = fields.GetField<TextField>("text").Value;
            texts = fields.GetField<TextField>("texts").Values;

            /* *******************************************************************************
             * Accessing ComponentLinkField, MultimediaLinkField
             * ***************************************************************************** */

            Component component = fields.GetComponent("link");
            IList<Component> components = fields.GetComponents("links");

            // Alternatives.

            component = fields.GetField<ComponentLinkField>("link").Value;
            components = fields.GetField<ComponentLinkField>("links").Values;

            /* *******************************************************************************
             * Accessing EmbeddedSchemaField.
             * ***************************************************************************** */

            ItemFields embeddedField = fields.GetEmbeddedField("paragraphs");
            IList<ItemFields> embeddedFields = fields.GetEmbeddedFields("paragraphs");

            // Alternatives.

            embeddedField = fields.GetField<EmbeddedSchemaField>("paragraphs").Value;
            embeddedFields = fields.GetField<EmbeddedSchemaField>("paragraphs").Values;

            /* *******************************************************************************
             * Accessing DateField.
             * ***************************************************************************** */

            DateTime publishDate = fields.GetDate("publish_date");
            IList<DateTime> publishDates = fields.GetDates("pubish_date");

            // Alternatives.

            publishDate = fields.GetField<DateField>("publish_date").Value;
            publishDates = fields.GetField<DateField>("publish_date").Values;

            /* *******************************************************************************
             * Accessing NumberField.
             * ***************************************************************************** */

            double height = fields.GetNumber("height");
            IList<double> heights = fields.GetNumbers("height");

            // Alternatives.

            height = fields.GetField<NumberField>("height").Value;
            heights = fields.GetField<NumberField>("height").Values;

            /* *******************************************************************************
             * Accessing KeywordField.
             * ***************************************************************************** */

            Keyword tag = fields.GetKeyword("tag");
            IList<Keyword> tags = fields.GetKeywords("tag");

            // Alternatives.

            tag = fields.GetField<KeywordField>("tag").Value;
            tags = fields.GetField<KeywordField>("tag").Values;

            return string.Empty;
        }
    }
}