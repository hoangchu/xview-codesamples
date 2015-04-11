using XView;

using XViewCodeSamples.Controllers;

namespace XViewCodeSamples.DecorationFilters
{
    public class CountryAndLanguageCodesDecorationFilter : OutputDecorationFilter
    {
        public CountryAndLanguageCodesDecorationFilter(SampleContext context)
        {
            this.Context = context;
        }

        private SampleContext Context { get; set; }

        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            // Return true to handle decoration filtering on all View output types.

            return true;
        }

        public override string Decorate(string text)
        {
            return text.Replace("{country}", this.Context.Configuration.CountryCode)
                .Replace("{language}", this.Context.Configuration.LanguageCode);
        }
    }
}