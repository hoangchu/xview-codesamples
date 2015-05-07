using System;
using System.Text;

using Tridion.ContentManager.ContentManagement;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    public class PageScopeView : View<Component>
    {
        protected override string Render()
        {
            // Sharing data between the different template scopes that are parts
            // of a Page (i.e. PT and all CTs on a Page) can be done through using
            // the this.Context.PageScope object.

            // IMPORTANT NOTE:
            // Information sharing between different template scopes only works when
            // publishing staticly.

            // In case of dynamic publishing the PageScope data will not be shared.
            // SDL Tridion decided to disable this since Tridion 2011 release, which is
            // I think a very remarkable decision. There are two type of dynamic CTs 
            // afterall. One of which can be placed on a page.

            // In case you need to share information between the different template scopes
            // in a dynamic publishing scenario (you're probably having a CWA implementation),
            // check out the method IntranetContext.GetCustomPageScope() and the accompanying 
            // View classes DynamicComponentView and DynamicPageView to see how PageScope data  
            // can be shared between templates in a dynamic publishing scenario.

            // The code below demonstrates a common scenario in which a CT renders all the
            // component presentations on a Page.

            if (this.Context.PageScope.ContainsKey("SkipRender"))
            {
                // If rendering already occurred, then skip rendering by returning an empty string.

                return string.Empty;
            }

            // Set skip rendering so other subsequent component presentations can check and 
            // ignore rendering by returning an emtpy string. (See code above).

            this.Context.PageScope.Add("SkipRender", true);

            var sb = new StringBuilder();

            foreach (var presentation in this.Context.Page.ComponentPresentations)
            {
                // This check ensures all component presentations on the page are from
                // the same type as the current presentation. (i.e. all presentations have
                // the same CT). Remove the check below if that's not a requirement.

                if (presentation.ComponentTemplate.Id != this.Context.Template.Id)
                {
                    throw new Exception(string.Format("Cannot have a presentation with CT \"{0}\"on this page.",
                        presentation.ComponentTemplate.Title));
                }

                sb.Append(this.Context.Engine.RenderComponentPresentation(presentation.Component.Id,
                    presentation.ComponentTemplate.Id));
            }

            return sb.ToString();
        }
    }
}