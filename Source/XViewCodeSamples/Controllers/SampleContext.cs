using System.Globalization;

using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;
using Tridion.ContentManager.Publishing.Rendering;
using Tridion.ContentManager.Templating;

using XView;

using XViewCodeSamples.Configurations;

namespace XViewCodeSamples.Controllers
{
    /// <summary>
    /// Represents the Context Object for the Tridion blueprint in which the Controller
    /// is handling Compound Templates.
    /// 
    /// Every implementation/blueprint should provide a Context class deriving from the
    /// XView's TridionContext class. This Context class defines properties and methods 
    /// expressing the rules/behaviours and configurations that are applicable to the 
    /// blueprint this Context class is created for.
    /// 
    /// An extended TridionContext class is not mandatory. Instead, you can use the TridionContext 
    /// provided by XView as-is if that's sufficient for your need. This can be the case 
    /// if you're dealing with a very small Tridion implementation.
    /// </summary>
    public class SampleContext : TridionContext
    {
        private const string PublicationConfigurationKey = "publicationconfiguration";
        private PublicationConfiguration configuration;
        private TemplatingLogger logger;

        private TemplatingLogger Logger
        {
            get { return this.logger ?? (this.logger = TemplatingLogger.GetLogger(this.GetType())); }
        }

        public PublicationConfiguration Configuration
        {
            get
            {
                // The code below demonstrates the usage of the ContextScope.
                // PublicationConfiguration can be cached in the ContextScope so it can
                // be used by multiple templates rendered in the same publish transaction
                // within a publication.

                // If your situation allows you to you can decide to cache the PublicationConfiguration
                // object in the cache using this.Cache for significant performance gain. 
                // Default cache life time is 15 minutes, but you can change this for each cache item.

                if (this.configuration != null)
                {
                    return this.configuration;
                }

                if (this.ContextScope.Contains(PublicationConfigurationKey))
                {
                    this.configuration = (PublicationConfiguration)this.ContextScope.Get(PublicationConfigurationKey);
                }
                else
                {
                    this.configuration =
                        new PublicationConfiguration(
                            this.Publication.GetMetadataField<ComponentLinkField>("publication_configuration").Value);

                    this.ContextScope.Set(PublicationConfigurationKey, this.configuration);
                }

                return this.configuration;
            }
        }

        public bool IsDynamicTemplate(Template template)
        {
            if (template is PageTemplate)
            {
                // Define implementation specific logic to determine whether a Page is
                // dynamic or not.

                // A PT in Tridion is static by nature. However, in some implementations some
                // specific PTs are designated to publish dynamic pages containing dynamic
                // component presentations. In such case the definition of a "dynamic" PT
                // can vary. Therefore apply appropriate logic below to determine a dynamic PT.

                // The code sample below says a PT that has the extension "somedynamicextension"
                // is a dynamic PT.

                return ((PageTemplate)template).FileExtension.Equals("somedynamicextension");
            }

            return ((ComponentTemplate)template).IsRepositoryPublishable;
        }

        /// <summary>
        /// Shortcut to Engine.PublishingContext.RenderedItem.AddBinary(Component multimediaComponent);
        /// </summary>
        /// <param name="multimediaComponent">Multimedia component.</param>
        /// <returns>Binary object.</returns>
        public Binary AddBinary(Component multimediaComponent)
        {
            return this.Engine.PublishingContext.RenderedItem.AddBinary(multimediaComponent);
        }

        /// <summary>
        /// This custom PageScope (which is an IDataContainer) allows PageScope data sharing among
        /// PT and all CTs on the same Page is a dynamic publishing scenario.
        /// 
        /// If your Tridion implementation does not deal with dynamic publishing or does not
        /// have the need to share data among templates on the sample Page in a dynamic publishing
        /// scenario, then omit this method. The default PageScope provided by XView should be
        /// sufficient in that case.
        /// </summary>
        /// <returns>IDataContainer representing the PageScope.</returns>
        protected override IDataContainer GetCustomPageScope()
        {
            const string pageScopeKeyPrefix = "custompagescope|{5F9B1A11-C541-42E1-89D2-AB8CE91AAFA7}|";

            var pageScopeKey = this.Page != null
                ? string.Format("{0}|{1}", pageScopeKeyPrefix, this.Page.Id.GetVersionlessUri())
                : string.Format("{0}|{1}|{2}", pageScopeKeyPrefix, this.Component.Id, this.Template.Id);

            IDataContainer pageScope;

            if (this.ContextScope.Contains(pageScopeKey))
            {
                pageScope = (IDataContainer)this.ContextScope.Get(pageScopeKey);
            }
            else
            {
                pageScope = new DataContainer();
                this.ContextScope.Set(pageScopeKey, pageScope);
            }

            if (!(this.Template is PageTemplate) || this.Page == null)
            {
                return pageScope;
            }

            foreach (var presentation in this.Page.ComponentPresentations)
            {
                if (!this.IsDynamicTemplate(presentation.ComponentTemplate))
                {
                    continue;
                }

                var presentationPageScopeKey = string.Format("{0}|{1}|{2}", pageScopeKeyPrefix, presentation.Component.Id,
                    presentation.ComponentTemplate.Id);

                this.ContextScope.Set(presentationPageScopeKey, pageScope);
            }

            return pageScope;
        }

        /// <summary>
        /// Example of how to generate a cache key for a cache item representing resolved
        /// data that is associated with a VersionedItem. (Example of VersionedItems are: Page,
        /// Component, ComponentTemplate, PageTemplate, etc.).
        /// </summary>
        /// <param name="versionedItem">VersionItem object.</param>
        /// <param name="keyPrefix">Optional cache key prefix.</param>
        /// <param name="publicationTargetRelevant">Should cache key be associated with a PublicationTarget?</param>
        /// <returns>String representing the cache key.</returns>
        private string GenerateCacheKey(VersionedItem versionedItem, string keyPrefix = null,
            bool publicationTargetRelevant = false)
        {
            keyPrefix = keyPrefix ?? "versioneditem";
            var objectId = versionedItem.Id.GetVersionlessUri().ToString();
            var versionId = versionedItem.Version.ToString(CultureInfo.InvariantCulture);

            if (!publicationTargetRelevant)
            {
                return string.Format("{0}|{1}|{2}", keyPrefix, objectId, versionId);
            }

            var targetId = this.IsPublishing && this.Engine.PublishingContext.PublicationTarget != null
                ? this.Engine.PublishingContext.PublicationTarget.Id.ToString()
                : "preview";

            return string.Format("{0}|{1}|{2}|{3}", keyPrefix, objectId, versionId, targetId);
        }
    }
}