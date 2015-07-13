using System;
using System.IO;
using System.Text.RegularExpressions;

using Chimote.Tridion.Templating.Intranet.Controllers;

using Tridion.ContentManager.ContentManagement;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public class ImagePathResolver
    {
        private static TridionLogger logger;
        private Regex imagePathRegex;
        private Regex imagePathTrailRegex;

        public ImagePathResolver(IntranetContext context)
        {
            this.Context = context;
        }

        private IntranetContext Context { get; set; }

        private Regex ImagePathRegex
        {
            get
            {
                return this.imagePathRegex ??
                       (this.imagePathRegex =
                           new Regex("(@import\\s+)?(url\\(|src=)(\"|')?([^\\),',\"]+)(\"|')?\\)?",
                               RegexOptions.Multiline | RegexOptions.Compiled));
            }
        }

        private Regex ImagePathTrailRegex
        {
            get
            {
                return this.imagePathTrailRegex ??
                       (this.imagePathTrailRegex = new Regex(@"(#|\?).*$", RegexOptions.Multiline | RegexOptions.Compiled));
            }
        }

        private static TridionLogger Logger
        {
            get { return logger ?? (logger = new TridionLogger(typeof(ComponentImage))); }
        }

        public string Resolve(string text, string contextFolderWebDavUrl)
        {
            var resolvedCss = text;
            var matches = this.ImagePathRegex.Matches(text);

            foreach (Match match in matches)
            {
                if (!match.Value.Contains("data:") && !match.Value.Contains("@import"))
                {
                    var originalPath = this.ImagePathRegex.Replace(match.Value, "$4");

                    if (!this.ImagePathIsResolvable(originalPath))
                    {
                        continue;
                    }

                    originalPath = this.ImagePathTrailRegex.Replace(originalPath, "");
                    var resolvedPath = this.ResolveImagePath(originalPath, contextFolderWebDavUrl);
                    resolvedCss = resolvedCss.Replace(originalPath, resolvedPath);
                }
            }

            return resolvedCss;
        }

        private string ResolveImagePath(string imagePath, string contextFolderWebDavUrl)
        {
            DebugGuard.ArgumentIsNotNullOrEmpty(imagePath, "imagePath");
            DebugGuard.ArgumentIsNotNullOrEmpty(contextFolderWebDavUrl, "contextFolderWebDavUrl");

            Logger.Debug("Resolving image path: " + imagePath);

            var imageWebDavUrl = this.ConstructImageWebDavUrl(imagePath, contextFolderWebDavUrl);
            var imageComponent = this.Context.Engine.GetObject<Component>(imageWebDavUrl);

            if (imageComponent == null)
            {
                var errorMessage = string.Format("Resolved image WebDavUrl \"{0}\" does not exist.", imageWebDavUrl);
                throw new Exception(errorMessage);
            }

            var imageUrl = this.Context.CreateCachedMultimediaItem(this.Context.Engine.GetObject<Component>(imageWebDavUrl)).Url;

            return imageUrl;
        }

        private string ConstructImageWebDavUrl(string imagePath, string contextFolderWebDavUrl)
        {
            DebugGuard.ArgumentIsNotNullOrEmpty(imagePath, "imagePath");
            DebugGuard.ArgumentIsNotNullOrEmpty(contextFolderWebDavUrl, "contextFolderWebDavUrl");

            var imagePathParts = imagePath.Split('/');
            var imagePathPartsCount = imagePathParts.Length;

            var containerWebDavUrlParts = contextFolderWebDavUrl.Split('/');
            var containerWebDavUrlPartsCount = containerWebDavUrlParts.Length;
            var trailingImagePath = string.Empty;

            for (var i = 0; i < imagePathPartsCount; i++)
            {
                if (imagePathParts[i] == "..")
                {
                    containerWebDavUrlPartsCount--;
                }
                else
                {
                    trailingImagePath += "/" + imagePathParts[i];
                }
            }

            var imageWebDavUrl = string.Empty;

            for (var i = 0; i < containerWebDavUrlPartsCount; i++)
            {
                if (containerWebDavUrlParts[i] != string.Empty)
                {
                    imageWebDavUrl += "/" + containerWebDavUrlParts[i];
                }
            }

            return imageWebDavUrl + trailingImagePath;
        }

        private bool ImagePathIsResolvable(string imagePath)
        {
            DebugGuard.ArgumentIsNotNullOrEmpty(imagePath, "imagePath");

            if (imagePath.StartsWith("http") ||
                imagePath.StartsWith("/") ||
                imagePath.StartsWith("#") ||
                imagePath.StartsWith("$") ||
                Regex.IsMatch(imagePath, @"[^A-Z,a-z,0-9,_,\-,/,\.]") ||
                string.IsNullOrEmpty(Path.GetExtension(imagePath)))
            {
                return false;
            }

            var fileExtension = Path.GetExtension(imagePath);

            return fileExtension.ToLower() != "css" && fileExtension.ToLower() != "js";
        }
    }
}