#nullable enable
using System.Linq;
using Xylab.Blogging.Entities;

namespace Xylab.Blogging.Services
{
    /// <summary>
    /// The <see cref="IQueryable{T}"/> store.
    /// </summary>
    public interface IBloggingQueryableStore
    {
        /// <summary>
        /// The queryable store for <see cref="BlogPost"/>s.
        /// </summary>
        IQueryable<BlogPost> Posts { get; }

        /// <summary>
        /// The queryable store for <see cref="BlogComment"/>s.
        /// </summary>
        IQueryable<BlogComment> Comments { get; }

        /// <summary>
        /// The queryable store for <see cref="BlogCommentVote"/>s.
        /// </summary>
        IQueryable<BlogCommentVote> CommentVotes { get; }

        /// <summary>
        /// The queryable store for <see cref="BlogPostVote"/>s.
        /// </summary>
        IQueryable<BlogPostVote> PostVotes { get; }

        /// <summary>
        /// The queryable store for <see cref="BlogRevision"/>s.
        /// </summary>
        IQueryable<BlogRevision> Revisions { get; }
    }
}
