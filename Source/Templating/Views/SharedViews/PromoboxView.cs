using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.SharedViews
{
    public class PromoboxView : ComponentView
    {
        protected override string Render()
        {
            dynamic xt = this.NewXTemplate(Layout.PromoboxView);
            xt.Title = this.Fields.GetText("title");
            xt.Introduction = this.Fields.GetText("introduction");
            xt.Text = this.Fields.GetText("text");

            var image = this.Fields.GetComponent("image");
            xt.ImageUrl = this.Context.PublishBinaryAndReturnUrl(image);
            xt.ImageAlt = image.GetMetadataField<TextField>("alt").Value;

            var link = this.Fields.GetComponent("link");
            xt.LinkUri = link.Id;
            xt.LinkText = link.GetText("title");

            return xt.ToString();
        }
    }
}