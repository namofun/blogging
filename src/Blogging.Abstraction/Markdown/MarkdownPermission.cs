using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace Blogging.Services
{
    /// <summary>
    /// The permission enum.
    /// </summary>
    [Flags]
    public enum MarkdownPermission
    {
        None = 0,
        NoHtml = 1,
        NoLinks = 2,
        NoAll = NoLinks | NoHtml
    }

    /// <summary>
    /// The markdown permission extension methods.
    /// </summary>
    public static class MarkdownPermissionExtensions
    {
        private class HtmlEscapedRenderer : HtmlBlockRenderer
        {
            protected override void Write(HtmlRenderer renderer, HtmlBlock obj)
            {
                renderer.WriteLeafRawLines(obj, true, true);
            }
        }

        private class HtmlEscapedInlineRenderer : HtmlInlineRenderer
        {
            protected override void Write(HtmlRenderer renderer, HtmlInline obj)
            {
                renderer.WriteEscape(obj.Tag);
            }
        }

        private class ProtectedLinkInlineRenderer : LinkInlineRenderer
        {
            protected override void Write(HtmlRenderer renderer, LinkInline link)
            {
                if (link.Url.StartsWith("//") || !link.Url.StartsWith("/"))
                {
                    if (link.IsImage) renderer.Write('!');
                    renderer.Write('[');
                    renderer.WriteChildren(link);
                    renderer.Write(']');
                    renderer.Write('(').WriteEscape(link.Url).Write(')');
                }
                else
                {
                    base.Write(renderer, link);
                }
            }
        }

        /// <summary>
        /// Disable HTML rendering and make it plain text.
        /// </summary>
        /// <param name="renderer">The object renderer collection.</param>
        /// <param name="disable">Whether to disable html rendering.</param>
        /// <returns>The original collection to chain.</returns>
        public static ObjectRendererCollection DisableHtml(this ObjectRendererCollection renderer, bool disable = true)
        {
            if (!disable) return renderer;
            renderer.Replace<HtmlBlockRenderer>(new HtmlEscapedRenderer());
            renderer.Replace<HtmlInlineRenderer>(new HtmlEscapedInlineRenderer());
            return renderer;
        }

        /// <summary>
        /// Disable link rendering and make it plain text.
        /// </summary>
        /// <param name="renderer">The object renderer collection.</param>
        /// <param name="disable">Whether to disable link rendering.</param>
        /// <returns>The original collection to chain.</returns>
        public static ObjectRendererCollection DisableLink(this ObjectRendererCollection renderer, bool disable = true)
        {
            if (!disable) return renderer;
            renderer.Replace<LinkInlineRenderer>(new ProtectedLinkInlineRenderer());
            return renderer;
        }

        /// <summary>
        /// Fix code rendering.
        /// </summary>
        /// <param name="renderer">The object renderer collection.</param>
        /// <returns>The original collection to chain.</returns>
        public static ObjectRendererCollection FixCodeRender(this ObjectRendererCollection renderer)
        {
            renderer.ReplaceOrAdd<CodeBlockRenderer>(new CodeBlockRenderer());
            return renderer;
        }
    }
}
