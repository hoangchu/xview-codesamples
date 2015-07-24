using System.Diagnostics;

using Chimote.Tridion.Templating.Intranet.Common;
using Chimote.Tridion.Templating.Intranet.Controllers;
using Chimote.Tridion.Templating.Intranet.Views.SystemViews;

using Tridion.ContentManager.CommunicationManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views
{
    /// <summary>
    /// Represents the base View for this project. All projects should define a base
    /// View like this one. Other Views should derive from this base View (directly or indirectly).
    /// 
    /// In this base View define common View properties and methods to be re-used 
    /// in derived Views. Or define centralized initializations or operations that 
    /// all derived Views will inherit from.
    /// </summary>
    public abstract class View<TModel> : View<IntranetContext, TModel>
    {
        private TridionLogger logger;
        private IntranetPageData pageData;

        protected TridionLogger Logger
        {
            get { return this.logger ?? (this.logger = new TridionLogger(this.GetType())); }
        }

        protected IntranetPageData PageData
        {
            get
            {
                if (this.pageData == null)
                {
                    const string pageDataKey = "pagedata";

                    if (this.Context.PageScope.ContainsKey(pageDataKey))
                    {
                        this.pageData = (IntranetPageData)this.Context.PageScope[pageDataKey];
                    }
                    else
                    {
                        this.pageData = new IntranetPageData();
                        this.Context.PageScope.Add(pageDataKey, this.pageData);
                    }
                }

                return this.pageData;
            }
        }

        /// <summary>
        /// PreRender() method coming from XView allows users to do pre-render actions.
        /// Examples of pre-render actions are writing debug info, check preconditions of
        /// the Render() method, etc..
        /// </summary>
        protected sealed override void PreRender()
        {
            this.WriteDebugInfo();
            this.CheckPreconditions();
        }

        /// <summary>
        /// Provides the ability to do centralized initialization logics.
        /// </summary>
        protected override void InitializeRender()
        {
            // Do some shared initialization here.

            // Derived views can override this method and call base.InitializeRender()
            // prior to derived specific logics.
        }

        /// <summary>
        /// PostRender() method coming from XView allows users to do post-render actions.
        /// An example post rendering action is to validate postcoditions of the Render()
        /// method.
        /// </summary>
        /// <param name="viewOutput">Decorated and validated view output.</param>
        protected sealed override void PostRender(string viewOutput)
        {
            this.CheckPostconditions(viewOutput);
        }

        /// <summary>
        /// Provides the ability to do precoditions check during the PreRender() phase.
        /// Override this method to do preconditions check for individual views.
        /// </summary>
        protected virtual void CheckPreconditions()
        {
            // PreconditionsCheck() gets executed in between InitializeRender() and Render().
        }

        /// <summary>
        /// Provides the ability to do postconditions check after output decoration and validation.
        /// Override this method to do postconditions check for individual views.
        /// </summary>
        /// <param name="viewOutput">Decorated and validated view output.</param>
        protected virtual void CheckPostconditions(string viewOutput)
        {
            // PostconditionsCheck() gets executed after output decoration and validation are done.
        }

        protected dynamic NewXTemplate(string template)
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

        [Conditional("DEBUG")]
        private void WriteDebugInfo()
        {
            this.Logger.Debug(new string('=', 100));
            this.WriteDebugInfoLine("ViewFullName", this.GetType().FullName);
            this.WriteDebugInfoLine("ParentView", this.Parent != null ? this.Parent.GetType().FullName : "null");
            this.WriteDebugInfoLine("EnableOutputDecoration", this.EnableOutputDecoration.ToString().ToLower());
            this.WriteDebugInfoLine("EnableOutputValidation", this.EnableOutputValidation.ToString().ToLower());
            this.WriteDebugInfoLine("TemplateName", this.Context.Template.Title);
            this.WriteDebugInfoLine("TemplateType", this.Context.Template is PageTemplate ? "Page" : "Component" + "Template");
            this.WriteDebugInfoLine("Publication", this.Context.Publication.Title);

            // Etc.
        }

        private void WriteDebugInfoLine(string infoName, string infoValue)
        {
            this.Logger.Debug(string.Format("{0} = {1}", infoName, infoValue));
        }
    }
}