using System;
using System.Xml;

using Chimote.Tridion.Templating.Intranet.Common;
using Chimote.Tridion.Templating.Intranet.Resources;

namespace Chimote.Tridion.Templating.Intranet.Configurations
{
    public class EnvironmentConfiguration
    {
        public EnvironmentConfiguration(string environment)
        {
            Guard.ArgumentIsNotNullOrEmpty(environment, "environment");

            this.LoadEnvironmentConfiguration(environment);
        }

        public string Name { get; private set; }
        public string ContentManagerUrl { get; private set; }
        public string LiveHostName { get; private set; }
        public string MediaManagerHostName { get; private set; }
        public string StagingHostName { get; private set; }
        public string StagingTargetUri { get; private set; }
        public string TempFolderPath { get; private set; }
        public string PreviewUrl { get; private set; }
        public string StagingUrl { get; private set; }
        public string StagingSecureUrl { get; private set; }
        public string LiveUrl { get; private set; }
        public string LiveSecureUrl { get; private set; }

        private void LoadEnvironmentConfiguration(string environment)
        {
            DebugGuard.ArgumentIsNotNullOrEmpty(environment, "environment");

            var configDocument = new XmlDocument();
            configDocument.LoadXml(ResourceFiles.EnvironmentConfiguration);

            this.Name = environment;
            environment = environment.ToLower();
            this.ContentManagerUrl = GetEnvironmentConfigValue(configDocument, environment, "ContentManagerUrl");
            this.LiveHostName = GetEnvironmentConfigValue(configDocument, environment, "LiveHostname");
            this.MediaManagerHostName = GetEnvironmentConfigValue(configDocument, environment, "MediaManagerHostname");
            this.StagingHostName = GetEnvironmentConfigValue(configDocument, environment, "StagingHostname");
            this.StagingTargetUri = GetEnvironmentConfigValue(configDocument, environment, "StagingTargetUri");
            this.TempFolderPath = GetEnvironmentConfigValue(configDocument, environment, "TempFolderPath");

            this.StagingUrl = "http://" + this.StagingHostName;
            this.PreviewUrl = this.StagingUrl;
            this.StagingSecureUrl = "https://" + this.StagingHostName;
            this.LiveUrl = "http://" + this.LiveHostName;
            this.LiveSecureUrl = "https://" + this.LiveHostName;
        }

        private static string GetEnvironmentConfigValue(XmlDocument configDocument, string environment, string configKey)
        {
            DebugGuard.ArgumentIsNotNull(configDocument, "configDocument");
            DebugGuard.ArgumentIsNotNullOrEmpty(environment, "environment");
            DebugGuard.ArgumentIsNotNullOrEmpty(configKey, "configKey");

            var xpath = string.Format("/config/{0}/entry[@key='{1}']", environment, configKey);
            var xmlNode = configDocument.SelectSingleNode(xpath);

            if (xmlNode == null)
            {
                throw new Exception(
                    string.Format(
                        "Could not find configuration entry {0} for environment {1} in Environment Configuration XML.",
                        configKey,
                        environment));
            }

            return xmlNode.InnerText;
        }
    }
}