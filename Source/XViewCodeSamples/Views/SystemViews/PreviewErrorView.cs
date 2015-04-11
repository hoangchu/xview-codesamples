using XView;

namespace XViewCodeSamples.Views.SystemViews
{
    public class PreviewErrorView : View<InvalidOutputContext>
    {
        protected override void InitializeRender()
        {
            base.InitializeRender();
            this.EnableOutputDecoration = false;
            this.EnableOutputValidation = false;
        }

        protected override string Render()
        {
            switch (this.Model.OutputType)
            {
                case ViewOutputType.Html:
                    this.Model.ExceptionHandled = true;
                    return this.RenderHtmlPreviewError();
                case ViewOutputType.Css:
                case ViewOutputType.Json:
                    // Etc.
                default:
                    // Exception is not handled.

                    return this.Model.ViewOutput;
            }
        }

        private string RenderHtmlPreviewError()
        {
            dynamic xt = this.NewXTemplate(SystemLayout.PreviewErrorView);
            xt.ViewOutput = this.Model.ViewOutput;
            xt.PreviewErrorMessage = this.Model.Exception.Message;

            if (this.Context.Page == null)
            {
                xt.Parse("root.previewErrorPrologue");
                xt.Parse("root.previewErrorEpilogue");
            }

            return xt.ToString();
        }
    }
}