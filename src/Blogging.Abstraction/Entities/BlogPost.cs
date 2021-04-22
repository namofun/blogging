using System;
using System.Collections.Generic;

namespace Blogging.Entities
{
    /// <summary>
    /// The entity class for blogging posts.
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// The post ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user ID of author
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Whether to display on home page
        /// </summary>
        public bool CommonShared { get; set; }

        /// <summary>
        /// The content HTML to display
        /// </summary>
        public string ContentHtml { get; set; }

        /// <summary>
        /// Whether this comment is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The revision number
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// The title of post
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The count of up votes
        /// </summary>
        public int UpVotes { get; set; }

        /// <summary>
        /// The count of down votes
        /// </summary>
        public int DownVotes { get; set; }

        /// <summary>
        /// The publish time of this post
        /// </summary>
        public DateTimeOffset PublishTime { get; set; }

        /// <summary>
        /// The replying comments
        /// </summary>
        /// <remarks>This property is used only in store, not in persist layer.</remarks>
        public ICollection<BlogComment> Comments { get; set; }

        /// <summary>
        /// The user information
        /// </summary>
        /// <remarks>This property is used only in store, not in persist layer.</remarks>
        public (int? Rating, string UserName, string Email) UserDetail { get; set; }
    }
}
