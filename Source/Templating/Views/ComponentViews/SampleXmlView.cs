using Tridion.ContentManager;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.ComponentViews
{
    /// <summary>
    /// This is a sample View for output XML. This sample View is for illustrating how to
    /// specify a specific ViewOutputType for a View for output filtering.
    /// 
    /// In this View override the OutputType property (which is by default ViewOutputType.Html) 
    /// to return ViewOutputType.Xml.
    /// </summary>
    public class SampleXmlView : ComponentView
    {
        /// <summary>
        /// Override the base OutputType to specify ViewOutputType.Xml.
        /// 
        /// If output decoration filtering and validation filtering are enabled, and
        /// if there are decoration filters and/or validation filters that are registered
        /// that do filtering of ViewOutputType.Xml, then the output of this View will
        /// be filtered by those filters.
        /// </summary>
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Xml; }
        }

        protected override string Render()
        {
            // Dummy code.

            return this.Model.ToXml(XmlFormat.R5Compatible).OuterXml;
        }
    }
}