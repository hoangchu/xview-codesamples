using System;

using Tridion.ContentManager.ContentManagement;

using XView;

namespace Chimote.Tridion.Templating.Intranet.Views.CodeSampleViews
{
    /// <summary>
    /// This example is the same as XTemplateExample1View, but with dynamic XTemplate.
    /// </summary>
    public class XTemplateExample1DynamicView : View<Component>
    {
        protected override string Render()
        {
            // Instantiates a new XTemplate object with a parameter which is the
            // content of the embedded file XTemplateExample1View.html. The content
            // of this file is exposed by the resources file Layout through the property
            // Layout.XTemplateExample1View

            dynamic xt = new XTemplate(Layout.XTemplateExample1View);

            // Assigns User.Description to variable {UserName}.
            // Assigns DateTime.Now.ToString("D") to variable {CurrentDate}.

            xt.UserName = this.Model.Session.User.Description;
            xt.CurrentDate = DateTime.Now.ToString("D");

            // Parses the "administrator" block, if the current user is Administrator.

            if (this.Model.Session.User.IsSystemAdministrator)
            {
                // Assigns value "God" to variable {Role}.

                xt.Role = "Root";

                // Block "administrator" is a child block of the "root" block.
                // The "root" block is the most outer block, which is a special
                // block. You don't have to explicitly specify the "root" block
                // in the markup.

                // The given block name for the .Parse() method has to be a fully
                // qualified block name. In this case "root.administrator".

                xt.Parse("root.administrator");
            }

            /* *******************************
             * Calling the .ToString() method without parameter returns the parsed "root" block
             * and is equal to the following two statements:
             * 
             * xt.Parse("root");
             * xt.ToString("root");
             * 
             * Or the following statement:
             * 
             * xt.ToString("root");
             * 
             ******************************* */

            return xt.ToString();
        }
    }
}