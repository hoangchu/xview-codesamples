using System;
using System.Collections.Generic;

using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    public class ExtensionMethodsComponentView : View<Component>
    {
        protected override string Render()
        {
            // Model is a Component.

            // For access individual content fields from a Component the extension methods
            // for ItemFields are also made available for Component. This makes makes accessing
            // a single field convenient. 

            // In case you need to access more than one fields from a Component you should
            // get the ItemFields (var fields = Model.GetFields();) then access the individual 
            // fields from the ItemFields object. This will make your code execute faster.

            // Below are sample codes for accessing different ItemField types from ItemFields.

            /* *******************************************************************************
             * Accessing TextField, XhtmlField, SingleLineTextField, MultiLineTextField, ExternalLinkField
             * ***************************************************************************** */

            // IMPORTANT NOTE: when a handler for the TridionExtensions.XhtmlResolver delegate
            // is assigned .GetText() will automatically resolve xhtml content from an XhtmlField.
            // See IntranetController how the TridionExtensions.XhtmlResolver is assigned.

            string text = this.Model.GetText("text");
            IList<string> texts = this.Model.GetTexts("texts");

            text = this.Model.GetField<TextField>("text").Value;
            texts = this.Model.GetField<TextField>("texts").Values;

            /* *******************************************************************************
             * Accessing ComponentLinkField, MultimediaLinkField
             * ***************************************************************************** */

            Component component = this.Model.GetComponent("link");
            IList<Component> components = this.Model.GetComponents("links");

            // Alternatives.

            component = this.Model.GetField<ComponentLinkField>("link").Value;
            components = this.Model.GetField<ComponentLinkField>("links").Values;

            /* *******************************************************************************
             * Accessing EmbeddedSchemaField.
             * ***************************************************************************** */

            ItemFields embeddedField = this.Model.GetEmbeddedField("paragraphs");
            IList<ItemFields> embeddedFields = this.Model.GetEmbeddedFields("paragraphs");

            // Alternatives.

            embeddedField = this.Model.GetField<EmbeddedSchemaField>("paragraphs").Value;
            embeddedFields = this.Model.GetField<EmbeddedSchemaField>("paragraphs").Values;

            /* *******************************************************************************
             * Accessing DateField.
             * ***************************************************************************** */

            DateTime publishDate = this.Model.GetDate("publish_date");
            IList<DateTime> publishDates = this.Model.GetDates("pubish_date");

            // Alternatives.

            publishDate = this.Model.GetField<DateField>("publish_date").Value;
            publishDates = this.Model.GetField<DateField>("publish_date").Values;

            /* *******************************************************************************
             * Accessing NumberField.
             * ***************************************************************************** */

            double height = this.Model.GetNumber("height");
            IList<double> heights = this.Model.GetNumbers("height");

            // Alternatives.

            height = this.Model.GetField<NumberField>("height").Value;
            heights = this.Model.GetField<NumberField>("height").Values;

            /* *******************************************************************************
             * Accessing KeywordField.
             * ***************************************************************************** */

            Keyword tag = this.Model.GetKeyword("tag");
            IList<Keyword> tags = this.Model.GetKeywords("tag");

            // Alternatives.

            tag = this.Model.GetField<KeywordField>("tag").Value;
            tags = this.Model.GetField<KeywordField>("tag").Values;

            // ===============================================================================
            // Below are extension methods for accessing individual metadata fields directly
            // from a Component.
            // ===============================================================================

            // Instead of accessing individual fields directly from a Component it is recommended
            // to get the metadata fields from the Component then access the individual fields
            // from the ItemFields object. 
            // For example:

            ItemFields metadata = this.Model.GetMetadataFields();
            string altText = metadata.GetText("alt");
            // Etc.. See ExtensionMethodsItemFieldsView for further info.

            /* *******************************************************************************
             * Accessing TextField, XhtmlField, SingleLineTextField, MultiLineTextField, ExternalLinkField
             * ***************************************************************************** */

            // IMPORTANT NOTE: when a handler for the TridionExtensions.XhtmlResolver delegate
            // is assigned .GetText() will automatically resolve xhtml content from an XhtmlField.
            // See IntranetController how the TridionExtensions.XhtmlResolver is assigned.

            text = this.Model.GetMetadataField<TextField>("text").Value;
            texts = this.Model.GetMetadataField<TextField>("texts").Values;

            /* *******************************************************************************
             * Accessing ComponentLinkField, MultimediaLinkField
             * ***************************************************************************** */

            component = this.Model.GetMetadataField<ComponentLinkField>("link").Value;
            components = this.Model.GetMetadataField<ComponentLinkField>("links").Values;

            /* *******************************************************************************
             * Accessing EmbeddedSchemaField.
             * ***************************************************************************** */

            embeddedField = this.Model.GetMetadataField<EmbeddedSchemaField>("paragraphs").Value;
            embeddedFields = this.Model.GetMetadataField<EmbeddedSchemaField>("paragraphs").Values;

            /* *******************************************************************************
             * Accessing DateField.
             * ***************************************************************************** */

            publishDate = this.Model.GetMetadataField<DateField>("publish_date").Value;
            publishDates = this.Model.GetMetadataField<DateField>("publish_date").Values;

            /* *******************************************************************************
             * Accessing NumberField.
             * ***************************************************************************** */

            height = this.Model.GetMetadataField<NumberField>("height").Value;
            heights = this.Model.GetMetadataField<NumberField>("height").Values;

            /* *******************************************************************************
             * Accessing KeywordField.
             * ***************************************************************************** */

            tag = this.Model.GetMetadataField<KeywordField>("tag").Value;
            tags = this.Model.GetMetadataField<KeywordField>("tag").Values;

            return string.Empty;
        }
    }
}