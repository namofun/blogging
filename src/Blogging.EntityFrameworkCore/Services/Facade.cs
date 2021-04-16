using Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blogging.Services
{
    public partial class BloggingFacade<TUser, TContext> : IBloggingFacade, IBloggingQueryableStore
        where TUser : SatelliteSite.IdentityModule.Entities.User
        where TContext : DbContext
    {
        public IBlogStore Blogs => this;

        public ICommentStore Comments => this;

        public TContext Context { get; }

        public IMarkdownResolver Markdown { get; }

        IQueryable<BlogPost> IBloggingQueryableStore.Posts => Context.Set<BlogPost>();

        IQueryable<BlogComment> IBloggingQueryableStore.Comments => Context.Set<BlogComment>();

        IQueryable<BlogCommentVote> IBloggingQueryableStore.CommentVotes => Context.Set<BlogCommentVote>();

        IQueryable<BlogPostVote> IBloggingQueryableStore.PostVotes => Context.Set<BlogPostVote>();

        IQueryable<BlogRevision> IBloggingQueryableStore.Revisions => Context.Set<BlogRevision>();

        public BloggingFacade(TContext context, IMarkdownResolver markdown)
        {
            Context = context;
            Markdown = markdown;
        }
    }
}
