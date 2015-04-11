using Tridion.ContentManager.CommunicationManagement;

using XView;

using XViewCodeSamples.Common;
using XViewCodeSamples.Controllers;

namespace XViewCodeSamples.Views.SystemViews
{
    public class CustomControllerView : ControllerView<SampleContext>
    {
        public CustomControllerView(SampleContext context) : base(context)
        {
        }

        protected override void InitializeRender()
        {
            base.InitializeRender();
            this.EnableOutputDecoration = false;
            this.EnableOutputValidation = false;
        }

        protected override string Render()
        {
            dynamic xt = new XTemplate(SystemLayout.CustomControllerView);
            xt.UserName = this.Context.Engine.GetSession().User.Description;
            xt.TemplateName = this.Model.ViewMapper.TridionTemplate.Title;

            xt.TemplateTypeName = this.Model.ViewMapper.TridionTemplate is ComponentTemplate
                ? "component template"
                : "page template";

            xt.ErrorMessage = this.Model.ViewMapper.Success ? "Oops, view not found!" : "Oops, invalid template name!";
            xt.ControllerTypeName = this.Model.GetType().Name;
            xt.ViewFullTypeName = this.Model.ViewMapper.ViewFullTypeName;
            xt.ProjectNamespace = this.Model.ViewMapper.ProjectRootNamespace;
            xt.TemplateNameRegexPattern = this.Model.ViewMapper.TemplateNameRegexPattern;
            xt.Parse(!this.Model.ViewMapper.Success ? "root.invalidTemplateName" : "root.viewNotFound");

            if (!string.IsNullOrEmpty(this.Model.ProjectDocumentationUrl))
            {
                xt.ProjectDocumentationUrl = this.Model.ProjectDocumentationUrl;
                xt.Parse("root.projectDocumentation");
            }

            return xt.ToString();
        }
    }
}