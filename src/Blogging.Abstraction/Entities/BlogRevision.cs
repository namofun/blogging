using System;

namespace Blogging.Entities
{
    /// <summary>
    /// The entity class for revisions on posts or comments.
    /// </summary>
    public class BlogRevision
    {
        /// <summary>
        /// The unique ID for revisions
        /// </summary>
        public int RevisionId { get; set; }

        /// <summary>
        /// The belonging comment ID
        /// </summary>
        public int? CommentId { get; set; }

        /// <summary>
        /// The belonging post ID
        /// </summary>
        public int? PostId { get; set; }

        /// <summary>
        /// The revision
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The raw markdown for content
        /// </summary>
        public string ContentRaw { get; set; }

        /// <summary>
        /// The revision time
        /// </summary>
        public DateTimeOffset Time { get; set; }
    }
}
