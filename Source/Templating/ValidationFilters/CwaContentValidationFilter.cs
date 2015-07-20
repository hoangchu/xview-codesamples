using Chimote.Tridion.Templating.Intranet.Common;
using Chimote.Tridion.Templating.Intranet.Controllers;

using Tridion.ContentManager.CommunicationManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.ValidationFilters
{
    public class CwaContentValidationFilter : OutputValidationFilter
    {
        public CwaContentValidationFilter(IntranetContext context)
        {
            this.Context = context;
        }

        private IntranetContext Context { get; set; }

        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            // Handle all output types.

            return true;
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