#nullable enable

namespace Xylab.Blogging.Services
{
    /// <summary>
    /// The facade for blogging related things.
    /// </summary>
    public interface IBloggingFacade
    {
        /// <summary>
        /// The store for blogs
        /// </summary>
        IBlogStore Blogs { get; }

        /// <summary>
        /// The store for comments
        /// </summary>
        ICommentStore Comments { get; }
    }
}
