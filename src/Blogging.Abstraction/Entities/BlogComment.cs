using System;
using System.Collections.Generic;

namespace Blogging.Entities
{
    /// <summary>
    /// The entity class for blogging comments.
    /// </summary>
    public class BlogComment
    {
        /// <summary>
        /// The comment ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The belonging post ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The user ID of author
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The content HTML to display
        /// </summary>
        public string ContentHtml { get; set; }

        /// <summary>
        /// Whether this comment is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The replied comment ID
        /// </summary>
        public int? ReplyToId { get; set; }

        /// <summary>
        /// The revision number
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// The count of up votes
        /// </summary>
        public int UpVotes { get; set; }

        /// <summary>
        /// The count of down votes
        /// </summary>
        public int DownVotes { get; set; }

        /// <summary>
        /// The publish time of this comment
        /// </summary>
        public DateTimeOffset PublishTime { get; set; }

        /// <summary>
        /// The replying comments
        /// </summary>
        /// <remarks>This property is used only in store, not in persist layer.</remarks>
        public ICollection<BlogComment> Replies { get; set; }

        /// <summary>
        /// The user information
        /// </summary>
        /// <remarks>This property is used only in store, not in persist layer.</remarks>
        public (int? Rating, string UserName, string Email) UserDetail { get; set; }
    }
}
