using System;
using System.Collections.Generic;

namespace Blogging.Entities
{
    public class BlogComment
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 博客文章ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 显示内容的HTML
        /// </summary>
        public string ContentHtml { get; set; }

        /// <summary>
        /// 是否已经被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 回复的评论
        /// </summary>
        public int? ReplyToId { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// 支持数
        /// </summary>
        public int UpVotes { get; set; }

        /// <summary>
        /// 反对数
        /// </summary>
        public int DownVotes { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTimeOffset PublishTime { get; set; }

        /// <summary>
        /// [Ignored] 所有回复
        /// </summary>
        public ICollection<BlogComment> Replies { get; set; }

        /// <summary>
        /// [Ignored] 用户信息
        /// </summary>
        public (int? Rating, string UserName, string Email) UserDetail { get; set; }
    }
}
