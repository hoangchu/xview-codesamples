using System.Collections.Generic;
using System.Xml;

using Chimote.Tridion.Templating.Intranet.Common;

using Tridion.ContentManager;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Publishing;
using Tridion.ContentManager.Templating;

using XView;

using ICache = XView.ICache;

namespace Chimote.Tridion.Templating.Intranet.Modules.Navigation
{
    public class SiteNavigation
    {
        private readonly Engine engine;
        private ICache cache;

        public SiteNavigation(Engine engine)
        {
            this.engine = engine;
        }

        public SiteNavigation(Engine engine, ICache cache)
        {
            this.engine = engine;
            this.Cache = cache;
        }

        private ICache Cache
        {
            get { return this.cache ?? (this.cache = new DefaultMemoryCache()); }
            set { this.cache = value; }
        }

        public XmlElement GetListPublishedPages(Publication publication, bool allowCache = true)
        {
            Guard.Requires(publication != null);

            var cacheKey = string.Format("publishedpages|{0}|{1}", publication.Id,
                this.engine.PublishingContext.PublicationTarget.Id);

            if (allowCache)
            {
                if (this.cache.Contains(cacheKey))
                {
                    return (XmlElement)this.cache.Get(cacheKey);
                }
            }

            var filter = new PublishedItemsFilter(this.engine.GetSession());
            filter.ForPublication = publication;
            filter.PublicationTarget = this.engine.PublishingContext.PublicationTarget;

            XmlElement publishedItems = PublishEngine.GetListPublishedItems(filter);
            var namespaceManager = XmlUtilities.CreateTridionNamespaceManager(publishedItems.OwnerDocument.NameTable);

            foreach (XmlElement publishedItem in publishedItems.SelectNodes("//tcm:Item", namespaceManager))
            {
                if (publishedItem.GetAttribute("Type") != "64")
                {
                    publishedItem.ParentNode.RemoveChild(publishedItem);
                }
            }

            this.cache.Set(cacheKey, publishedItems);

            return publishedItems;
        }

        public XmlElement GetListStructureGroups(Publication publication)
        {
            Guard.Requires(publication != null);

            var cacheKey = string.Format("structuregroups|{0}", publication.Id);

            if (this.Cache.Contains(cacheKey))
            {
                return (XmlElement)this.Cache.Get(cacheKey);
            }

            var filter = new OrganizationalItemsFilter(this.engine.GetSession());
            filter.ItemTypes = new List<ItemType> { ItemType.StructureGroup };
            filter.BaseColumns = ListBaseColumns.Extended;

            XmlElement structureGroups = publication.GetListOrganizationalItems(filter);
            this.Cache.Set(cacheKey, structureGroups);

            return structureGroups;
        }
    }
}