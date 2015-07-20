using System;

using XView;

namespace Chimote.Tridion.Templating.Intranet.ValidationFilters
{
    public class TestContentValidationFilter : OutputValidationFilter
    {
        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            // Validate all output types for test/dummy content.

            return true;
        }

        public override void Validate(string text)
        {
            if (text.ToLower().Contains("lorem ipsum"))
            {
                throw new Exception("Output contains test content");
            }
        }
    }
}