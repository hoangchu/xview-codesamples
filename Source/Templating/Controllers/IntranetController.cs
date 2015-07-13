using System;

using Chimote.Tridion.Templating.Intranet.DecorationFilters;
using Chimote.Tridion.Templating.Intranet.ValidationFilters;
using Chimote.Tridion.Templating.Intranet.Views.SystemViews;

using Tridion.ContentManager.Templating;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Controllers
{
    /// <summary>
    /// Represents the single-point-of-contact (interface) with Tridion's Compound Templating solution.
    /// 
    /// Each Tridion blueprint only needs one Controller to handle all templates in that blueprint.
    /// Having more than one Controller in a blueprint is possible, but highly discouraged. Keep the
    /// dependency to Tridion as minimum as possible for a better development/maintenance experience.
    /// 
    /// Notice the base Controller of type IntranetContext. The IntranetContext is a type derived from the
    /// XView.TridionContext class. See the IntranetContext.cs for further info regarding the Context Object.
    /// </summary>
    public class IntranetController : Controller<IntranetContext>
    {
        /// <summary>
        /// This method is the entry point of the XView execution flow from the implementation
        /// point of view. Define global initialization logics in here. This is also the place 
        /// where you can perform global validation and restriction checks should your 
        /// implementation have such requirements.
        /// </summary>
        protected override void InitializeViewRequest()
        {
            this.RegisterXhtmlResolver();

            // Registers decoration filters to do view output decorations.
            // The queue of decoration filters gets processed first, followed
            // by the validation filters queue.
            // The filters get executed in the order in which they're registered.

            this.RegisterOutputFilter(new CountryAndLanguageCodesDecorationFilter(this.Context.Configuration.CountryCode,
                this.Context.Configuration.LanguageCode));

            this.RegisterOutputFilter(new DefaultFinishActionsDecorationFilter(this.Context));
            
            // Etc.

            this.RegisterOutputFilter(new HtmlValidationFilter(this.Context));
            this.RegisterOutputFilter(new XmlValidationFilter());
            this.RegisterOutputFilter(new JsonValidationFilter());

            // Etc.
        }

        /// <summary>
        /// Provides a custom ControllerView to show project/company specific content when
        /// the Controller cannot map a Template with a View.
        /// 
        /// Two examples scenarios amongst possible scenarios in which you may want to have
        /// a custom ControllerView:
        /// 
        /// 1. If you have a custom implementation of IViewMapper in which you'd enforce
        ///    a different template naming convention. In this case you want to provide
        ///    information describing that naming convention in the custom ControllerView.
        /// 
        /// 2. If you want to have a different layout and/or want to provide additonal
        ///    documentation/information to your developers when the ControllerView is shown.
        /// </summary>
        protected override ControllerView<IntranetContext> GetCustomControllerView()
        {
            return new IntranetControllerView(this.Context);
        }

        /// <summary>
        /// Provides a custom ViewMapper here should you need to extend the View mapping
        /// rules.
        /// In small to medium sized projects you probably don't need this. The default
        /// ViewMapper should be sufficient. In that case remove this method.
        /// </summary>
        protected override IViewMapper GetCustomViewMapper()
        {
            return new IntranetViewMapper(this.GetRootNamespace());
        }

        /// <summary>
        /// This is the hook where you can implement a model mapper to provide custom ViewModels.
        /// In general XView term I believe custom ViewModels for entry Views would introduce extra 
        /// overhead without any gain. Therefore I see no need for a "model mapper" in normal circustances.
        /// 
        /// In addition, a View class in XView is a C# class and is powerful. It can be utilized as
        /// a controller to dispatch render requests to other Views using partial render.
        ///
        /// However, if you'd run into scenarios in which employing custom ViewModels for entry Views 
        /// would be benificial, then feel free to do so!
        /// </summary>
        protected override object GetCustomViewModel()
        {
            // Provide your model mapping logic here.

            // Your options are:
            // - Map a ViewModel based on the mapped View.
            //   In this case you have this.ViewType to figure out the mapped View.

            // - Map a ViewModel based on the default model (which is a Component or Page) 
            //   In this case check if this.Context.Template is ComponentTemplate or PageTemplate.
            //   Base on the result you can get either this.Context.Component or this.Context.Page
            //   respectively.

            // - Etc.

            // You may want to utilize the memory cache (this.Context.Cache) to keep track
            // of a mapped table for speedy model mapping on subsequent mappings.

            return null;
        }

        /// <summary>
        /// This method is only needed if XView is installed in the GAC. If you'd merge XView.dll
        /// with your project's dll (using ILMerge.exe) you can remove this method. However, it cannot 
        /// hurt to have this method either way.
        /// </summary>
        protected override Type GetInternalType(string typeFullName)
        {
            return Type.GetType(typeFullName);
        }

        /// <summary>
        /// Assigns a static method to the static TridionExtensions.XhtmlResolver delegate to do
        /// auto xhtml resolving when using .GetText() extension method.
        /// 
        /// IMPORTANT INFORMATION: 
        /// Do not assign an instance method to the TridionExntensions.XhtmlResolver delegate.
        /// This will lead to unpredictable behaviours/results and possible memory leak.
        /// </summary>
        private void RegisterXhtmlResolver()
        {
            if (TridionExtensions.XhtmlResolver == null)
            {
                TridionExtensions.XhtmlResolver = TemplateUtilities.ResolveRichTextFieldXhtml;
            }
        }
    }
}