using System;

using Chimote.Tridion.Templating.Intranet.Controllers;

using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views
{
    /// <summary>
    /// Represents the base View for rendering Page model that contains
    /// dynamic Component Presentations.
    /// </summary>
    public abstract class DynamicPageView : PageView
    {
        private IViewMapper viewMapper;

        private IViewMapper ViewMapper
        {
            get { return this.viewMapper ?? (this.viewMapper = new IntranetViewMapper("Chimote.Tridion.Templating.Intranet")); }
        }

        /// <summary>
        /// Maps a ComponentTemplate with DynamicComponentView. This method is part of a workaround
        /// to allow dynamic CTs to share PageScope data with PT and all subsequent CTs on the same
        /// Page.
        /// </summary>
        /// <param name="template">ComponentTemplate.</param>
        /// <returns>DynamicComponentView object.</returns>
        private DynamicComponentView GetDynamicComponentView(ComponentTemplate template)
        {
            if (this.ViewMapper.MapView(template).Success)
            {
                var viewType = Type.GetType(this.ViewMapper.ViewFullTypeName);

                if (viewType != null && typeof(DynamicComponentView).IsAssignableFrom(viewType))
                {
                    var view = (DynamicComponentView)Activator.CreateInstance(viewType);
                    view.Context = this.Context;

                    return view;
                }
            }

            return null;
        }

        protected void PopulateDynamicPresentationPageData(ComponentPresentation presentation)
        {
            using (var view = this.GetDynamicComponentView(presentation.ComponentTemplate))
            {
                if (view != null)
                {
                    view.PopulatePageData(presentation);
                }
            }
        }

        /// <summary>
        /// Implements a workaround to alllow dynamic CTs to share PageScope data with the PT and all
        /// subsequent CTs on the same Page.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="template">ComponentTemplate.</param>
        /// <returns>String representing the render output of a Component Presentation.</returns>
        protected override string RenderPresentation(Component component, ComponentTemplate template)
        {
            this.PopulateDynamicPresentationPageData(new ComponentPresentation(component, template));

            return base.RenderPresentation(component, template);
        }
    }
}