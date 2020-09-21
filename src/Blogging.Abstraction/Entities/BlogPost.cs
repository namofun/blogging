using System;
using System.Collections.Generic;

namespace Blogging.Entities
{
    public class BlogPost
    {
        /// <summary>
        /// 博客文章ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 是否在首页显示
        /// </summary>
        public bool CommonShared { get; set; }

        /// <summary>
        /// 显示内容的HTML
        /// </summary>
        public string ContentHtml { get; set; }

        /// <summary>
        /// 是否已经被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

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
        /// [Ignored] 所有博客评论
        /// </summary>
        public ICollection<BlogComment> Comments { get; set; }

        /// <summary>
        /// [Ignored] 用户信息
        /// </summary>
        public (int? Rating, string UserName, string Email) UserDetail { get; set; }
    }
}
