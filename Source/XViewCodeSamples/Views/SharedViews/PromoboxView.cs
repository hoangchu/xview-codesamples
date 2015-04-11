using Tridion.ContentManager.ContentManagement.Fields;

using XView;

namespace XViewCodeSamples.Views.SharedViews
{
    public class PromoboxView : ComponentView
    {
        protected override string Render()
        {
            dynamic xt = this.NewXTemplate(SharedLayout.PromoboxView);
            xt.Title = this.Fields.GetText("title");
            xt.Introduction = this.Fields.GetText("introduction");
            xt.Text = this.Fields.GetText("text");

            var image = this.Fields.GetComponent("image");
            xt.ImageUrl = this.Context.AddBinary(image).Url;
            xt.ImageAlt = image.GetMetadataField<TextField>("alt").Value;

            var link = this.Fields.GetComponent("link");
            xt.LinkUri = link.Id;
            xt.LinkText = link.GetText("title");

            return xt.ToString();
        }
    }
}