using XView;

using XViewCodeSamples.Common;

namespace XViewCodeSamples.ValidationFilters
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