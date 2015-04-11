using Tridion.ContentManager.CommunicationManagement;

using XView;

using XViewCodeSamples.Common;
using XViewCodeSamples.Controllers;

namespace XViewCodeSamples.ValidationFilters
{
    public class HtmlValidationFilter : OutputValidationFilter
    {
        public HtmlValidationFilter(SampleContext context)
        {
            this.Context = context;
        }

        private SampleContext Context { get; set; }

        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            return viewOutputType == ViewOutputType.Html;
        }

        public override void Validate(string text)
        {
            // Example scenario of a CWA implementation where HTML output
            // from a dynamic CT has to be XML well-formed.

            if (this.Context.Template is ComponentTemplate && this.Context.IsDynamicTemplate(this.Context.Template))
            {
                XmlUtilities.ValidateXmlWellFormness(text);
            }
        }
    }
}