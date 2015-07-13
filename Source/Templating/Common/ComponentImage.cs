using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Publishing.Rendering;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public class ComponentImage : MultimediaItem
    {
        private static TridionLogger logger;
        private readonly IDictionary<string, string> attributes = new Dictionary<string, string>();
        private int? imageHeight;
        private int? imageWidth;

        public ComponentImage(Component component, RenderedItem renderedItem, bool forceDownloadExternal = false)
            : base(component, renderedItem, forceDownloadExternal)
        {
            this.InitializeImage();
        }

        private static TridionLogger Logger
        {
            get { return logger ?? (logger = new TridionLogger(typeof(ComponentImage))); }
        }

        /// <summary>
        /// Gets the width of the image. If image has no width, then Width is equal to Int32.MinValue.
        /// </summary>
        public int Width
        {
            get { return this.imageWidth.HasValue ? this.imageWidth.Value : Int32.MinValue; }
            protected set { this.imageWidth = value; }
        }

        /// <summary>
        /// Gets the height of the image. If image has no height, then Height property is equal to Int32.MinValue.
        /// </summary>
        public int Height
        {
            get { return this.imageHeight.HasValue ? this.imageHeight.Value : Int32.MinValue; }
            protected set { this.imageHeight = value; }
        }

        /// <summary>
        /// Gets the alt-text of the image.
        /// </summary>
        public string Alt
        {
            get { return this.attributes.ContainsKey("alt") ? this.attributes["alt"] : null; }
            private set
            {
                if (value == null)
                {
                    return;
                }

                if (this.attributes.ContainsKey("alt"))
                {
                    this.attributes["alt"] = value;
                }
                else
                {
                    this.attributes.Add("alt", value);
                }
            }
        }

        /// <summary>
        /// Gets the title of the image.
        /// </summary>
        public override string Title
        {
            get { return this.attributes.ContainsKey("title") ? this.attributes["title"] : null; }
            protected set
            {
                if (value == null)
                {
                    return;
                }

                if (this.attributes.ContainsKey("title"))
                {
                    this.attributes["title"] = value;
                }
                else
                {
                    this.attributes.Add("title", value);
                }
            }
        }

        /// <summary>
        /// Gets the URL of the image.
        /// </summary>
        public override string Url
        {
            get
            {
                if (!this.attributes.ContainsKey("src"))
                {
                    this.attributes.Add("src", base.Url);
                }

                return this.attributes["src"];
            }
        }

        private void InitializeImage()
        {
            if (this.Metadata != null)
            {
                if (this.Metadata.Contains("width") && this.Metadata.GetNumbers("width").Count > 0)
                {
                    this.Width = (int)this.Metadata.GetNumber("width");
                }

                if (this.Metadata.Contains("height") && this.Metadata.GetNumbers("height").Count > 0)
                {
                    this.Height = (int)this.Metadata.GetNumber("height");
                }

                if (this.Metadata.Contains("alt"))
                {
                    this.Alt = this.Metadata.GetText("alt");

                    if (string.IsNullOrEmpty(this.Alt))
                    {
                        this.Alt = this.Component.Title;
                    }
                }

                if (this.Metadata.Contains("title"))
                {
                    this.Title = this.Metadata.GetText("title");
                }
            }
        }

        /// <summary>
        /// Gets the width and height of the given imageComponent.
        /// </summary>
        /// <param name="imageComponent">Multimedia component containing an image.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>true if there is a width and height, otherwise false and width and height equal Int32.MinValue.</returns>
        public static bool TryGetImageDimension(Component imageComponent, out int width, out int height)
        {
            Guard.Ensures(imageComponent.ComponentType == ComponentType.Multimedia,
                "Component \"{0}\" ({1}) is not a multimedia component", imageComponent.Title, imageComponent.Id);

            width = Int32.MinValue;
            height = Int32.MinValue;

            try
            {
                using (var stream = new MemoryStream(imageComponent.BinaryContent.GetByteArray()))
                using (Image image = Image.FromStream(stream))
                {
                    width = image.Width;
                    height = image.Height;
                }
            }
            catch (Exception e)
            {
                Logger.Debug(string.Format("Could not get dimensions from image component. {0}", e));
                return false;
            }

            Logger.Debug(
                string.Format(
                    "Multimedia component \"{0}\" with tcmuri {1} has these dimensions: width={2}, height={3}",
                    imageComponent.Title,
                    imageComponent.Id,
                    width,
                    height));

            return true;
        }

        /// <summary>
        /// Sets an attribute for the image tag.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        public void SetAttribute(string name, string value)
        {
            var lowerCaseName = name.ToLower();

            if (lowerCaseName == "width" || lowerCaseName == "height" || lowerCaseName == "alt" || lowerCaseName == "title"
                || lowerCaseName == "src")
            {
                throw new Exception("Cannot set the following reserved attributes: src, width, height, alt, title.");
            }

            if (this.attributes.ContainsKey(name))
            {
                this.attributes[name] = value;
            }
            else
            {
                this.attributes.Add(name, value);
            }
        }

        /// <summary>
        /// Outputs the <img /> tag.
        /// </summary>
        /// <returns>String represents an <img /> tag.</returns>
        public override string ToString()
        {
            // This is a hack to trigger a "src" attribute to be added with this.Url
            // as value. The .Url property must be refactored.

            var imageUrl = this.Url;

            var sb = new StringBuilder();
            sb.Append("<img ");

            foreach (var attribute in this.attributes)
            {
                sb.AppendFormat("{0}=\"{1}\" ", attribute.Key, attribute.Value.Replace("\"", "&quot;"));
            }

            if (this.Width != Int32.MinValue)
            {
                sb.AppendFormat("width=\"{0}\" ", this.Width);
            }

            if (this.Height != Int32.MinValue)
            {
                sb.AppendFormat("height=\"{0}\" ", this.Height);
            }

            sb.Append("/>");

            return sb.ToString();
        }
    }
}