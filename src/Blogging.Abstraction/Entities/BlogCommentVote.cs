namespace Xylab.Blogging.Entities
{
    /// <summary>
    /// The entity class for voting comments.
    /// </summary>
    public class BlogCommentVote
    {
        /// <summary>
        /// The comment ID
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Whether the vote is up or down
        /// </summary>
        public bool Up { get; set; }

        /// <summary>
        /// The post ID
        /// </summary>
        /// <remarks>Redundant for fetching all the vote results.</remarks>
        public int PostId { get; set; }
    }
}
