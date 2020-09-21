using Markdig;
using Markdig.Renderers;
using System.IO;

namespace Blogging.Services
{
    /// <summary>
    /// The permission markdown resolver.
    /// </summary>
    public interface IMarkdownResolver
    {
        /// <summary>
        /// Render the raw content with given permission.
        /// </summary>
        /// <param name="rawContent">The raw content.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>The rendered HTML.</returns>
        string Render(string rawContent, MarkdownPermission permission);
    }

    /// <summary>
    /// The default markdig resolver.
    /// </summary>
    public class DefaultMarkdownResolver : IMarkdownResolver
    {
        private readonly IMarkdownService _service;

        public DefaultMarkdownResolver(IMarkdownService service)
        {
            _service = service;
        }

        public string Render(string rawContent, MarkdownPermission permission)
        {
            var doc = _service.Parse(rawContent);
            using var textWriter = new StringWriter();
            var renderer = new HtmlRenderer(textWriter);
            _service.Pipeline.Setup(renderer);

            renderer.ObjectRenderers
                .DisableHtml((permission & MarkdownPermission.NoHtml) == MarkdownPermission.NoHtml)
                .DisableLink((permission & MarkdownPermission.NoLinks) == MarkdownPermission.NoLinks)
                .FixCodeRender();

            renderer.Render(doc);
            renderer.Writer.Flush();
            return textWriter.ToString();
        }
    }
}
