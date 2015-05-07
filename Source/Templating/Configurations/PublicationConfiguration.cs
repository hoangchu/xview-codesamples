using Tridion.ContentManager.ContentManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Configurations
{
    /// <summary>
    /// Represents a dummy publication configuration for illustrating XView. Publication configuration
    /// is common to many multi-lingual Tridion implementations in which publication specific configurations
    /// are defined Tridion and exposed in and used by template codes.
    /// 
    /// It is recommended to expose the PublicationConfiguration object as a public property of
    /// the TridionContext derived class. In this case the IntranetContext class.
    /// </summary>
    public class PublicationConfiguration
    {
        public PublicationConfiguration(Component configuration)
        {
            this.CountryCode = configuration.GetText("country");
            this.LanguageCode = configuration.GetText("language");
        }

        public string CountryCode { get; private set; }
        public string LanguageCode { get; private set; }
    }
}