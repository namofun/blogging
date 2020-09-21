namespace Blogging.Entities
{
    public class BlogCommentVote
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// 投票用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public bool Up { get; set; }

        /// <summary>
        /// 对应博文ID的冗余字段
        /// </summary>
        public int PostId { get; set; }
    }
}
