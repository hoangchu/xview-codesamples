using System;

using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;

using XView;

using XViewCodeSamples.Controllers;

namespace XViewCodeSamples.Views
{
    public abstract class DynamicPageView : PageView
    {
        private IViewMapper viewMapper;

        private IViewMapper ViewMapper
        {
            get { return this.viewMapper ?? (this.viewMapper = new CustomViewMapper("XViewCodeSamples")); }
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

                if (viewType != null)
                {
                    if (typeof(DynamicComponentView).IsAssignableFrom(viewType))
                    {
                        var view = (DynamicComponentView)Activator.CreateInstance(viewType);
                        view.Context = this.Context;

                        return view;
                    }
                }
            }

            return null;
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
            using (var view = this.GetDynamicComponentView(template))
            {
                if (view != null)
                {
                    view.SetPageScopeData(component);
                }
            }

            return base.RenderPresentation(component, template);
        }
    }
}