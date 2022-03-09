namespace Xylab.Blogging.Entities
{
    /// <summary>
    /// The entity class for voting posts.
    /// </summary>
    public class BlogPostVote
    {
        /// <summary>
        /// The post ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Whether the vote is up or down
        /// </summary>
        public bool Up { get; set; }
    }
}
