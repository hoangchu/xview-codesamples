using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.PageViews
{
    public class JsonView : PageView
    {
        public override ViewOutputType OutputType
        {
            get { return ViewOutputType.Json; }
        }

        protected override void InitializeRender()
        {
            base.InitializeRender();
            this.EnableOutputDecoration = true;
            this.EnableOutputValidation = true;
        }

        protected override string Render()
        {
            return this.RenderPresentations();
        }
    }
}