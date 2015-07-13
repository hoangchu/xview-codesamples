using Newtonsoft.Json.Linq;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.ComponentViews
{
    public class ContactLinksJsonView : ComponentView
    {
        protected override void InitializeRender()
        {
            base.InitializeRender();
            this.EnableOutputDecoration = true;
            this.EnableOutputValidation = false;
        }

        protected override string Render()
        {
            var linkArray = new JArray();

            foreach (var linkFields in this.Fields.GetEmbeddedFields("links"))
            {
                var linkText = linkFields.GetText("text");
                var linkUrl = linkFields.GetText("url");
                var linkCode = linkFields.GetText("code");

                var linkObject = new JObject
                {
                    new JProperty("title", linkText),
                    new JProperty("url", linkUrl),
                    new JProperty("code", linkCode)
                };

                linkArray.Add(linkObject);
            }

            var links = new JObject { new JProperty("links", linkArray) };

            return links.ToString();
        }
    }
}