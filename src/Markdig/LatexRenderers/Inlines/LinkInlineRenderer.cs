using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.LaTeX.Inlines
{
    public class LinkInlineRenderer : LatexObjectRenderer<LinkInline>
    {
        protected override void Write(LatexRenderer renderer, LinkInline link)
        {
            var finalLink = (link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url) ?? "";

            if (link.IsImage)
            {
                renderer.Write("\\includegraphics{").Write(finalLink).Write("}");
            }
            else
            {
                renderer.Write("\\texttt{").WriteEscape(finalLink).Write("}");
            }
        }
    }
}