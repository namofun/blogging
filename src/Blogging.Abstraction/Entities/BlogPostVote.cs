namespace Blogging.Entities
{
    public class BlogPostVote
    {
        /// <summary>
        /// 博客文章ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 投票用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public bool Up { get; set; }
    }
}
