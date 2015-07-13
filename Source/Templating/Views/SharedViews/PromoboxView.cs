using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.SharedViews
{
    public class PromoboxView : ComponentView
    {
        protected override string Render()
        {
            var xt = this.NewXTemplate(Layout.PromoboxView);
            xt.Title = this.Fields.GetText("title");
            xt.Introduction = this.Fields.GetText("introduction");
            xt.Text = this.Fields.GetText("text");

            var image = this.Context.CreateImage(this.Fields.GetComponent("image"));
            image.SetAttribute("class", "promo-image");
            xt.ImageTag = image.ToString();

            var link = this.Fields.GetComponent("link");
            xt.LinkUri = link.Id;
            xt.LinkText = link.GetText("title");

            return xt.ToString();
        }
    }
}