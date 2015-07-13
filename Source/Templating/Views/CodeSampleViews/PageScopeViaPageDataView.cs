using System;
using System.Text;

using Tridion.ContentManager.ContentManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    /// <summary>
    /// This example View demontrates the usage of Context.PageScope the same way as the
    /// PageScopeView does, but instead of access the Context.PageScope object directly
    /// it's making use of of the IntranetPageData exposed as View.PageData property.
    /// </summary>
    public class PageScopeViaPageDataView : View<Component>
    {
        protected override string Render()
        {
            if (this.PageData.SkipRender)
            {
                // If rendering already occurred, then skip rendering by returning an empty string.

                return string.Empty;
            }

            // Set skip rendering so other subsequent component presentations can check and 
            // ignore rendering by returning an emtpy string. (See code above).

            this.PageData.SkipRender = true;
            this.PageData.Title = this.Model.GetText("title");

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