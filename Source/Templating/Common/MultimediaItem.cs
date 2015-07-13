using System;
using System.IO;
using System.Text.RegularExpressions;

using Tridion.ContentManager;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.ContentManagement.Fields;
using Tridion.ContentManager.Publishing.Rendering;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public class MultimediaItem
    {
        private readonly bool forceDownloadExternal;
        private readonly RenderedItem renderedItem;

        private ItemFields metadata;
        private long? size;
        private string url;

        public MultimediaItem(Component component, RenderedItem renderedItem, bool forceDownloadExternal = false)
        {
            Guard.ArgumentIsNotNull(component, "component");
            Guard.ArgumentIsNotNull(renderedItem, "renderedItem");
            Guard.Ensures(component.ComponentType == ComponentType.Multimedia,
                "Specified component ({0}) is not a multimedia component", component.Id);

            this.Component = component;
            this.renderedItem = renderedItem;
            this.forceDownloadExternal = forceDownloadExternal;

            // Statement below is there to bypass a TOM.NET bug.
            // This statement makes sure that the Component is fully loaded.

            var bypassTomNetBug = this.Component.Creator;
        }

        protected Component Component { get; private set; }

        public TcmUri Id
        {
            get { return this.Component.Id; }
        }

        public virtual string Title
        {
            get { return this.Component.Title; }
            protected set { throw new NotSupportedException(); }
        }

        public virtual string Url
        {
            get
            {
                if (string.IsNullOrEmpty(this.url))
                {
                    this.url = this.PublishBinaryAndGetUrl();
                }

                return this.url;
            }
        }

        /// <summary>
        /// Get the size of the image in bytes.
        /// </summary>
        public long Size
        {
            get
            {
                if (!this.size.HasValue)
                {
                    this.size = this.Component.BinaryContent.IsExternal
                        ? this.Component.BinaryContent.GetByteArray().Length
                        : this.Component.BinaryContent.Size;
                }

                return this.size.Value;
            }
        }

        public bool IsExternal
        {
            get { return this.Component.BinaryContent.IsExternal; }
        }

        public bool ForceDownloadExternal
        {
            get { return this.forceDownloadExternal; }
        }

        public ItemFields Metadata
        {
            get { return this.metadata ?? (this.metadata = this.Component.GetMetadataFields()); }
        }

        private string PublishBinaryAndGetUrl()
        {
            if (this.IsExternal && !this.forceDownloadExternal)
            {
                return this.Component.BinaryContent.ExternalBinaryUri.AbsoluteUri;
            }

            var regex = new Regex(@"[^a-zA-z0-9\-_\.]", RegexOptions.Compiled | RegexOptions.Singleline);
            var originalFileName = this.Component.BinaryContent.Filename;

            if (!regex.IsMatch(originalFileName))
            {
                return this.renderedItem.AddBinary(this.Component).Url;
            }

            var fileNameParts = originalFileName.Split('.');

            if (fileNameParts.Length < 2)
            {
                var errorMessage = string.Format("Multimedia item {0} has an invalid file name: {1}", this.Component.Id,
                    originalFileName);

                throw new Exception(errorMessage);
            }

            var fileExtension = fileNameParts[fileNameParts.Length - 1];
            var fileName = originalFileName.Substring(0, originalFileName.Length - fileExtension.Length - 1);
            fileName = regex.Replace(fileName, "-");

            fileName = string.Format("{0}_tcm{1}-{2}.{3}", fileName, this.Component.Id.PublicationId, this.Component.Id.ItemId,
                fileExtension);

            using (var stream = new MemoryStream(this.Component.BinaryContent.GetByteArray()))
            {
                return this.renderedItem.AddBinary(stream, fileName,
                    "has-illegal-chars", this.Component, this.Component.BinaryContent.MultimediaType.MimeType).Url;
            }
        }
    }
}