using Chimote.Tridion.Templating.Intranet.Controllers;

using Tridion.ContentManager.Templating.Templates;

using XView;

namespace Chimote.Tridion.Templating.Intranet.DecorationFilters
{
    public class DefaultFinishActionsDecorationFilter : OutputDecorationFilter
    {
        public DefaultFinishActionsDecorationFilter(IntranetContext context)
        {
            this.Context = context;
        }

        private IntranetContext Context { get; set; }

        public override bool CanHandle(ViewOutputType viewOutputType)
        {
            // Apply Default Finish Actions decoration on Html output only.

            return viewOutputType == ViewOutputType.Html;
        }

        /// <summary>
        /// This method calls Tridion's built-in ITemplates that are used in Tridion's
        /// Default Finish Actions TBB.
        /// Here you can choose to omit calls to ITemplates that you don't need.
        /// In addition you can add conditinal checks to only perform decoration
        /// when necessary and/or to only perform decoration in PT View if the rendition
        /// occurs in a Page context (this.Context.Page != null) to avoid overhead of 
        /// executing Default Finsh Actions multiple times.
        /// </summary>
        public override string Decorate(string text)
        {
            text = this.Context.RenderExtractBinariesFromHtml(text);
            text = this.Context.RenderITemplate<ConvertXmlToHtmlTemplate>(text);
            text = this.Context.RenderITemplate<PublishBinariesInPackageTemplate>(text);
            text = this.Context.RenderITemplate<LinkResolverTemplate>(text);
            text = this.Context.RenderITemplate<TargetGroupPersonalizationTemplate>(text);
            text = this.Context.RenderITemplate<CleanupTemplate>(text);

            return text;
        }
    }
}