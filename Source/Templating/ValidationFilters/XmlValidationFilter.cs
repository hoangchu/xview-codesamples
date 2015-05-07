using Chimote.Tridion.Templating.Intranet.Common;

using XView;

namespace Chimote.Tridion.Templating.Intranet.ValidationFilters
{
    public class XmlValidationFilter : OutputValidationFilter
    {
        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            return viewOutputType == ViewOutputType.Xml;
        }

        public override void Validate(string text)
        {
            XmlUtilities.ValidateXmlWellFormness(text);
        }
    }
}