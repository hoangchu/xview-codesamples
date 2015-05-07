using Chimote.Tridion.Templating.Intranet.Common;
using Chimote.Tridion.Templating.Intranet.Controllers;

using Tridion.ContentManager.CommunicationManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.ValidationFilters
{
    public class HtmlValidationFilter : OutputValidationFilter
    {
        public HtmlValidationFilter(IntranetContext context)
        {
            this.Context = context;
        }

        private IntranetContext Context { get; set; }

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