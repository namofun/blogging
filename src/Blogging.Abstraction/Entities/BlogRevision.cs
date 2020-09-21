using System;

namespace Blogging.Entities
{
    public class BlogRevision
    {
        /// <summary>
        /// 博客修订内容
        /// </summary>
        public int RevisionId { get; set; }

        /// <summary>
        /// 评论ID
        /// </summary>
        public int? CommentId { get; set; }

        /// <summary>
        /// 博客文章ID
        /// </summary>
        public int? BlogId { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 原始博客内容
        /// </summary>
        public string ContentRaw { get; set; }

        /// <summary>
        /// 修订时间
        /// </summary>
        public DateTimeOffset Time { get; set; }
    }
}
