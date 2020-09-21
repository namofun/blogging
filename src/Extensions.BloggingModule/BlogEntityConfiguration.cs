﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SatelliteSite.Entities;

namespace Blogging.Entities
{
    public class BlogEntityConfiguration<TContext> :
        EntityTypeConfigurationSupplier<TContext>,
        IEntityTypeConfiguration<BlogPost>,
        IEntityTypeConfiguration<BlogPostVote>,
        IEntityTypeConfiguration<BlogComment>,
        IEntityTypeConfiguration<BlogCommentVote>,
        IEntityTypeConfiguration<BlogRevision>
        where TContext : DbContext
    {
        public void Configure(EntityTypeBuilder<BlogPost> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Ignore(e => e.Comments);
            entity.Ignore(e => e.UserDetail);
        }

        public void Configure(EntityTypeBuilder<BlogPostVote> entity)
        {
            entity.HasKey(e => new { e.PostId, e.UserId });

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void Configure(EntityTypeBuilder<BlogComment> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.ReplyToId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Ignore(e => e.Replies);
            entity.Ignore(e => e.UserDetail);
        }

        public void Configure(EntityTypeBuilder<BlogCommentVote> entity)
        {
            entity.HasKey(e => new { e.CommentId, e.UserId });

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
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
            entity.HasKey(e => e.RevisionId);

            entity.HasOne<BlogPost>()
                .WithMany()
                .HasForeignKey(e => e.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BlogComment>()
                .WithMany()
                .HasForeignKey(e => e.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
