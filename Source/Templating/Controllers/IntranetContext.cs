using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Chimote.Tridion.Templating.Intranet.Common;
using Chimote.Tridion.Templating.Intranet.Configurations;

using Tridion.ContentManager;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;
using Tridion.ContentManager.Templating;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Controllers
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
    public class IntranetContext : TridionContext
    {
        private PublicationConfiguration configuration;
        private IDictionary<string, string> labels;
        private TemplatingLogger logger;

        private TemplatingLogger Logger
        {
            get { return this.logger ?? (this.logger = TemplatingLogger.GetLogger(this.GetType())); }
        }

        private IDictionary<string, string> Labels
        {
            get { return this.labels ?? (this.labels = this.GetContextLabels()); }
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

                const string publicationConfigurationKey = "publicationconfiguration";

                if (this.ContextScope.ContainsKey(publicationConfigurationKey))
                {
                    this.configuration = (PublicationConfiguration)this.ContextScope[publicationConfigurationKey];
                }
                else
                {
                    this.configuration =
                        new PublicationConfiguration(
                            this.Publication.GetMetadataField<ComponentLinkField>("publication_configuration").Value);

                    this.ContextScope.Add(publicationConfigurationKey, this.configuration);
                }

                return this.configuration;
            }
        }

        public bool IsPageContext
        {
            get { return this.Page != null; }
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
        /// Shortcut to Engine.PublishingContext.RenderedItem.AddBinary(Component multimediaComponent).Url;
        /// </summary>
        /// <param name="multimediaComponent">Multimedia component.</param>
        /// <returns>String represents the URL.</returns>
        public string PublishBinaryAndReturnUrl(Component multimediaComponent)
        {
            Guard.Requires(multimediaComponent != null);
            Guard.Requires(multimediaComponent.ComponentType == ComponentType.Multimedia);

            return this.Engine.PublishingContext.RenderedItem.AddBinary(multimediaComponent).Url;
        }

        public string TranslateLabel(string label)
        {
            Guard.Requires(!string.IsNullOrEmpty(label));

            if (this.Labels.ContainsKey(label))
            {
                return this.Labels[label];
            }

            throw new Exception(string.Format("Label \"{0}\" does not exist", label));
        }

        /// <summary>
        /// Creates and returns a ComponentImage object for the given multimediaComponent.
        /// </summary>
        /// <param name="multimediaComponent">Multimedia component containing an image.</param>
        /// <param name="forceDownloadExternal">Force an external image to be downloaded and published.</param>
        /// <returns>ComponentImage object.</returns>
        public ComponentImage CreateImage(Component multimediaComponent, bool forceDownloadExternal = false)
        {
            Guard.ArgumentIsNotNull(multimediaComponent, "multimediaComponent");

            return new ComponentImage(multimediaComponent, this.Engine.PublishingContext.RenderedItem, forceDownloadExternal);
        }

        public MultimediaItem CreateMultimediaItem(Component multimediaItem, bool forceDownloadExternal = false)
        {
            Guard.ArgumentIsNotNull(multimediaItem, "multimediaItem");

            return new MultimediaItem(multimediaItem, this.Engine.PublishingContext.RenderedItem, forceDownloadExternal);
        }

        public MultimediaItem CreateCachedMultimediaItem(Component multimediaComponent)
        {
            Guard.ArgumentIsNotNull(multimediaComponent, "multimediaComponent");

            var cacheKey = this.GenerateCacheKey(multimediaComponent, keyPrefix: "multimedia", publicationTargetRelevant: true);

            if (this.Cache.Contains(cacheKey))
            {
                this.Logger.Debug(string.Format("Cache: get multimedia item with key {0} from cache.", cacheKey));

                return (MultimediaItem)this.Cache.Get(cacheKey);
            }

            var multimediaItem = this.CreateMultimediaItem(multimediaComponent);
            this.Logger.Debug(string.Format("Cache: set multimedia item with key {0} into cache.", cacheKey));
            this.Cache.Set(cacheKey, multimediaItem);

            return multimediaItem;
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
        /// <returns>IDictionary{string, object} representing the PageScope.</returns>
        protected override IDictionary<string, object> GetCustomPageScope()
        {
            const string pageScopeKeyPrefix = "custompagescope|{5F9B1A11-C541-42E1-89D2-AB8CE91AAFA7}|";

            var pageScopeKey = this.Page != null
                ? string.Format("{0}|{1}", pageScopeKeyPrefix, this.Page.Id.GetVersionlessUri())
                : string.Format("{0}|{1}|{2}", pageScopeKeyPrefix, this.Component.Id, this.Template.Id);

            IDictionary<string, object> pageScope;

            if (this.ContextScope.ContainsKey(pageScopeKey))
            {
                pageScope = (IDictionary<string, object>)this.ContextScope[pageScopeKey];
            }
            else
            {
                pageScope = new Dictionary<string, object>();
                this.ContextScope.Add(pageScopeKey, pageScope);
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

                this.ContextScope.Add(presentationPageScopeKey, pageScope);
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

        private IDictionary<string, string> GetContextLabels()
        {
            var cacheKey = string.Format("labels|{0}", this.Configuration.LanguageCode);

            if (this.Cache.Contains(cacheKey))
            {
                return (IDictionary<string, string>)this.Cache.Get(cacheKey);
            }

            var labelsFolderWebDavUrl = this.Publication.WebDavUrl + "/folder/containing/label/components";
            var labelsFolder = (Folder)this.Engine.GetObject(labelsFolderWebDavUrl);

            var filter = new OrganizationalItemItemsFilter(this.Engine.GetSession());
            filter.ItemTypes = new List<ItemType> { ItemType.Component };
            filter.Recursive = false;

            var labelComponents = labelsFolder.GetItems(filter).OfType<Component>();
            var contextLabels = new Dictionary<string, string>();

            foreach (var labelComponent in labelComponents)
            {
                if (labelComponent.Schema.Title == "Label")
                {
                    contextLabels.Add(labelComponent.Title, labelComponent.GetText("value"));
                }
            }

            this.Cache.Set(cacheKey, contextLabels);

            return contextLabels;
        }
    }
}