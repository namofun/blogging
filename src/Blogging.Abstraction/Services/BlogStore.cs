#nullable enable
using Blogging.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogging.Services
{
    /// <summary>
    /// The store conventions for blogs.
    /// </summary>
    public interface IBlogStore
    {
        /// <summary>
        /// Creates a blog post.
        /// </summary>
        /// <param name="uid">The user ID.</param>
        /// <param name="title">The post title.</param>
        /// <param name="rawContent">The markdown source.</param>
        /// <param name="share">Whether this blog post is shown on the home page.</param>
        /// <returns>The created post.</returns>
        Task<BlogPost> CreateAsync(int uid, string title, string rawContent, bool share);

        /// <summary>
        /// Sets whether the post is shown on the home page.
        /// </summary>
        /// <param name="post">The blog post.</param>
        /// <param name="common">Whether this blog post is shown on the home page.</param>
        /// <returns>Whether the operation is succeeded.</returns>
        Task<bool> SetCommonAsync(BlogPost post, bool common);

        /// <summary>
        /// Enlists the blog posts.
        /// </summary>
        /// <param name="byUser">The user ID, null if all.</param>
        /// <param name="count">The count of posts to take.</param>
        /// <param name="skip">The count of posts to skip.</param>
        /// <returns>The list of blog posts.</returns>
        Task<List<BlogPost>> ListAsync(int? byUser = null, int count = 10, int skip = 0);

        /// <summary>
        /// Finds the post with comments and other things.
        /// </summary>
        /// <param name="postId">The blog post ID.</param>
        /// <returns>The blog post.</returns>
        Task<BlogPost?> FindAsync(int postId);

        /// <summary>
        /// Directly finds the post without getting other informations.
        /// </summary>
        /// <param name="postId">The blog post ID.</param>
        /// <returns>The blog post.</returns>
        Task<BlogPost?> QuickFindAsync(int postId);

        /// <summary>
        /// Gets the last revision of specified blog post.
        /// </summary>
        /// <param name="post">The blog post to get from.</param>
        /// <returns>The lastest revision.</returns>
        Task<BlogRevision> GetLastRevisionAsync(BlogPost post);

        /// <summary>
        /// Gets the revisions of specified blog post.
        /// </summary>
        /// <param name="post">The blog post to get from.</param>
        /// <returns>The all revisions.</returns>
        Task<List<BlogRevision>> GetRevisionsAsync(BlogPost post);

        /// <summary>
        /// Revises the post with the content markdown.
        /// </summary>
        /// <param name="post">The post to revise.</param>
        /// <param name="title">The post title.</param>
        /// <param name="rawContent">The content markdown.</param>
        /// <param name="share">Whether this blog post is shown on the home page.</param>
        /// <returns>The created revision.</returns>
        Task<BlogRevision> ReviseAsync(BlogPost post, string title, string rawContent, bool share);

        /// <summary>
        /// Marks the post as deleted.
        /// </summary>
        /// <param name="post">The post to delete.</param>
        /// <returns>The task to delete post.</returns>
        Task DeleteAsync(BlogPost post);

        /// <summary>
        /// Votes the post.
        /// </summary>
        /// <param name="postId">The blog post ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="existing">The original status.</param>
        /// <param name="toBe">The status to be.</param>
        /// <returns>A boolean indicating whether operation succeeded.</returns>
        Task<bool> VoteAsync(int postId, int userId, bool? existing, bool? toBe);

        /// <summary>
        /// Gets the comment votes for certain posts from such user.
        /// </summary>
        /// <param name="blogIds">The blog post IDs.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>The dictionary for vote.</returns>
        Task<Dictionary<int, BlogPostVote>> StatisticsAsync(IEnumerable<int> blogIds, int userId);

        /// <summary>
        /// Statistics the votes for a certain post.
        /// </summary>
        /// <param name="blogId">The blog post ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <returns>The tuple of (UpVotes, DownVotes, CurrentVote).</returns>
        Task<(int, int, BlogPostVote)> StatisticsAsync(int blogId, int userId);
    }
}
