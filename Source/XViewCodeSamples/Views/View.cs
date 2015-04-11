using Tridion.ContentManager.Templating;

using XView;

using XViewCodeSamples.Common;
using XViewCodeSamples.Controllers;
using XViewCodeSamples.Views.SystemViews;

namespace XViewCodeSamples.Views
{
    /// <summary>
    /// Represents the base View for this project. All projects should define this
    /// base View from which all Views should be derived (directly or indirectly).
    /// 
    /// In this base View define common View properties and methods to be re-used 
    /// in derived Views.
    /// </summary>
    public abstract class View<TModel> : View<SampleContext, TModel>
    {
        private TemplatingLogger logger;

        protected TemplatingLogger Logger
        {
            get { return this.logger ?? (this.logger = TemplatingLogger.GetLogger(this.GetType())); }
        }

        protected XTemplate NewXTemplate(string template)
        {
            var xt = new XTemplate(template);

            // By default XTemplate replaces an unassigned variable with an empty string.
            // If you want to keep this behaviour, then remove the statement below.

            xt.ReserveUnassignedVariableTags = true;

            return xt;
        }

        protected override string HandleAndRenderInvalidOutputInPreviewMode(InvalidOutputContext invalidOutputContext)
        {
            return this.RenderPartial<PreviewErrorView>(invalidOutputContext);
        }
    }
}