using System;

using Newtonsoft.Json.Linq;

using XView;

namespace Chimote.Tridion.Templating.Intranet.ValidationFilters
{
    public sealed class JsonValidationFilter : OutputValidationFilter
    {
        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            return viewOutputType == ViewOutputType.Json;
        }

        public override void Validate(string text)
        {
            try
            {
                JToken.Parse(text);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Json output is not valid: {0}", ex.Message), ex);
            }
        }
    }
}