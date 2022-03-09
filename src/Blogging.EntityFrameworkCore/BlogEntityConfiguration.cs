using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xylab.Blogging.Entities
{
    public class BlogEntityConfiguration<TUser, TContext> :
        EntityTypeConfigurationSupplier<TContext>,
        IEntityTypeConfiguration<BlogPost>,
        IEntityTypeConfiguration<BlogPostVote>,
        IEntityTypeConfiguration<BlogComment>,
        IEntityTypeConfiguration<BlogCommentVote>,
        IEntityTypeConfiguration<BlogRevision>
        where TContext : DbContext
        where TUser : SatelliteSite.IdentityModule.Entities.User
    {
        public void Configure(EntityTypeBuilder<BlogPost> entity)
        {
            entity.ToTable("BlogPosts");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PostId");

            entity.HasOne<TUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Ignore(e => e.Comments);
            entity.Ignore(e => e.UserDetail);
        }

        public void Configure(EntityTypeBuilder<BlogPostVote> entity)
        {
            entity.ToTable("BlogPostVotes");

            entity.HasKey(e => new { e.PostId, e.UserId });

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<TUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void Configure(EntityTypeBuilder<BlogComment> entity)
        {
            entity.ToTable("BlogComments");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("CommentId");

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.ReplyToId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<TUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Ignore(e => e.Replies);
            entity.Ignore(e => e.UserDetail);
        }

        public void Configure(EntityTypeBuilder<BlogCommentVote> entity)
        {
            entity.ToTable("BlogCommentVotes");

            entity.HasKey(e => new { e.CommentId, e.UserId });

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<TUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PostId, e.UserId });
        }

        public void Configure(EntityTypeBuilder<BlogRevision> entity)
        {
            entity.ToTable("BlogRevisions");

            entity.HasKey(e => e.RevisionId);

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
