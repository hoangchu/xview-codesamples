using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tridion.ContentManager.ContentManagement;

using XView;

namespace XViewCodeSamples.Views.XTemplateCodeSamples
{
    public class XTemplateExample1View : View<Component>
    {
        protected override string Render()
        {
            XTemplate xt = new XTemplate("");

            xt.Assign("UserName", this.Model.Session.User.Description);


            return xt.ToString();
        }
    }
}
