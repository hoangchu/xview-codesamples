using Tridion.ContentManager.CommunicationManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Controllers
{
    public class IntranetViewMapper : ViewMapper
    {
        public IntranetViewMapper(string rootNamespace) : base(rootNamespace)
        {
        }

        protected override string GetViewNamespace()
        {
            if (this.TridionTemplate is ComponentTemplate)
            {
                switch (this.TridionTemplate.OrganizationalItem.Title)
                {
                    case "Homepage":
                        // Routing CTs in "Homepage" folder to "HomepageViews" sub-namespace.

                        return this.ProjectRootNamespace + "Views.HomepageViews";
                    case "Mobile":
                        // Routing CTs in "Mobile" folder to "MobileViews" sub-namespace.

                        return this.ProjectRootNamespace + "Views.MobileViews";
                    case "System":
                        // Routing CTs in "System" folder to "SystemViews" sub-namespace.

                        return this.ProjectRootNamespace + "Views.SystemViews";
                }
            }

            return base.GetViewNamespace();
        }

        // An alternative to the above is to do switch(this.TemplateNamePrefix) with the template name
        // prefixes to match with View sub-namespaces.
        // For example, the TemplateNamePrefix of a template named "Mobile: Promobox" is "Mobile".
        // The View that matchs with this template is ...Views.MobileViews.PromoboxView.
    }
}