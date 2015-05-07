using XView;

namespace Chimote.Tridion.Templating.Intranet.DecorationFilters
{
    public class CountryAndLanguageCodesDecorationFilter : OutputDecorationFilter
    {
        private readonly string countryCode;
        private readonly string languageCode;

        public CountryAndLanguageCodesDecorationFilter(string countryCode, string languageCode)
        {
            this.countryCode = countryCode;
            this.languageCode = languageCode;
        }

        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            // Return true to handle decoration filtering on all View output types.

            return true;
        }

        public override string Decorate(string text)
        {
            return text.Replace("{country}", this.countryCode).Replace("{language}", this.languageCode);
        }
    }
}