using System.Collections.Generic;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public class IntranetPageData
    {
        private readonly IList<string> scripts;
        private readonly IList<string> styles;

        public IntranetPageData()
        {
            this.styles = new List<string>();
            this.scripts = new List<string>();
        }

        public string Title { get; set; }
        public bool SkipRender { get; set; }

        public void AddScript(string scriptPath)
        {
            if (!string.IsNullOrEmpty(scriptPath) && !this.scripts.Contains(scriptPath))
            {
                this.scripts.Add(scriptPath);
            }
        }

        public void AddStyle(string cssPath)
        {
            if (!string.IsNullOrEmpty(cssPath) && !this.styles.Contains(cssPath))
            {
                this.styles.Add(cssPath);
            }
        }

        public IEnumerable<string> GetScripts()
        {
            foreach (var script in this.scripts)
            {
                yield return script;
            }
        }

        public IEnumerable<string> GetStyles()
        {
            foreach (var css in this.styles)
            {
                yield return css;
            }
        }
    }
}