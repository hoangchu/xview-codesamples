namespace XViewCodeSamples.Views.PageViews
{
    /// <summary>
    /// This View is a typical View to only output ComponentPresentations as-is which
    /// can be used / seen in nearly all Tridion implementations.
    /// 
    /// This View can be mapped by multiple page templates (PT) that only need to output
    /// the component presentations as-is. For examples PTs to output .html, .js, .css, .xml, etc. files.
    /// Those PTs can have names like this respectively:
    /// - Html: Render
    /// - Js: Render
    /// - Css: Render
    /// - Xml: Render
    /// </summary>
    public class RenderView : PageView
    {
        protected override string Render()
        {
            return this.RenderPresentations();
        }
    }
}