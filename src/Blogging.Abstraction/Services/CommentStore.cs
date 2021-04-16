#nullable enable
using Blogging.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogging.Services
{
    /// <summary>
    /// The store conventions for blogging comments.
    /// </summary>
    public interface ICommentStore
    {
        /// <summary>
        /// Creates a comment for the specified post.
        /// </summary>
        /// <param name="post">The post to reply to.</param>
        /// <param name="uid">The user ID.</param>
        /// <param name="contentMd">The markdown source of comment.</param>
        /// <param name="replyTo">The comment to reply to, null if directly to the post.</param>
        /// <returns>The created comment.</returns>
        Task<BlogComment> CreateAsync(BlogPost post, int uid, string contentMd, BlogComment? replyTo = null);

        /// <summary>
        /// Gets the last revision of specified blogging comment.
        /// </summary>
        /// <param name="comment">The comment to get from.</param>
        /// <returns>The lastest revision.</returns>
        Task<BlogRevision> GetLastRevisionAsync(BlogComment comment);

        /// <summary>
        /// Gets the revisions of specified blogging comment.
        /// </summary>
        /// <param name="comment">The comment to get from.</param>
        /// <returns>The all revisions.</returns>
        Task<List<BlogRevision>> GetRevisionsAsync(BlogComment comment);

        /// <summary>
        /// Revises the comment with the content markdown.
        /// </summary>
        /// <param name="comment">The comment to revise.</param>
        /// <param name="rawContent">The content markdown.</param>
        /// <returns>The created revision.</returns>
        Task<BlogRevision> ReviseAsync(BlogComment comment, string rawContent);

        /// <summary>
        /// Marks the comment as deleted.
        /// </summary>
        /// <param name="comment">The comment to delete.</param>
        /// <returns>The task to delete comment.</returns>
        Task DeleteAsync(BlogComment comment);

        /// <summary>
        /// Finds the corresponding blogging comment.
        /// </summary>
        /// <param name="postId">The post ID.</param>
        /// <param name="commentId">The comment ID.</param>
        /// <returns>The found comment.</returns>
        Task<BlogComment?> FindAsync(int postId, int commentId);

        /// <summary>
        /// Votes the comment.
        /// </summary>
        /// <param name="postId">The post ID.</param>
        /// <param name="commentId">The comment ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="existing">The original status.</param>
        /// <param name="toBe">The status to be.</param>
        /// <returns>A boolean indicating whether operation succeeded.</returns>
        Task<bool> VoteAsync(int postId, int commentId, int userId, bool? existing, bool? toBe);

        /// <summary>
        /// Gets the comment votes for a certain post from such user.
        /// </summary>
        /// <param name="blogId">The blog ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>The dictionary for vote.</returns>
        Task<Dictionary<int, BlogCommentVote>> StatisticsAsync(int blogId, int userId);

        /// <summary>
        /// Statistics the votes for a certain comment.
        /// </summary>
        /// <param name="blogId">The blog ID.</param>
        /// <param name="commentId">The comment ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>The tuple of (CurrentVote, UpVotes, DownVotes).</returns>
        Task<(BlogCommentVote?, int, int)> StatisticsAsync(int blogId, int commentId, int userId);
    }
}
