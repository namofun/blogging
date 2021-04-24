using Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public partial class BloggingFacade<TUser, TContext> : ICommentStore
    {
        public async Task<BlogComment> CreateAsync(BlogPost post, int uid, string contentMd, BlogComment replyTo)
        {
            var e = Context.Set<BlogComment>().Add(new BlogComment
            {
                PublishTime = DateTimeOffset.Now,
                PostId = post.Id,
                UserId = uid,
                ReplyToId = replyTo?.Id,
                ContentHtml = PreparingInternally,
            });

            await Context.SaveChangesAsync();
            await ReviseAsync(e.Entity, contentMd);
            return e.Entity;
        }

        public async Task<BlogRevision> ReviseAsync(BlogComment comment, string rawContent)
        {
            var e = Context.Set<BlogRevision>().Add(new BlogRevision
            {
                CommentId = comment.Id,
                ContentRaw = rawContent,
                Time = DateTimeOffset.Now,
                Version = comment.Revision + 1
            });

            await Context.SaveChangesAsync();

            var result = Markdown.Render(rawContent, MarkdownPermission.NoAll);
            comment.Revision++;
            comment.ContentHtml = result;
            Context.Set<BlogComment>().Update(comment);

            await Context.SaveChangesAsync();
            return e.Entity;
        }

        public Task<BlogRevision> GetLastRevisionAsync(BlogComment comment)
        {
            return Context.Set<BlogRevision>()
                .Where(r => r.CommentId == comment.Id && r.Version == comment.Revision)
                .SingleOrDefaultAsync();
        }

        public Task<List<BlogRevision>> GetRevisionsAsync(BlogComment comment)
        {
            return Context.Set<BlogRevision>()
                .Where(r => r.CommentId == comment.Id)
                .OrderBy(r => r.Version)
                .ToListAsync();
        }

        public Task DeleteAsync(BlogComment comment)
        {
            return Context.Set<BlogComment>()
                .Where(c => c.Id == comment.Id)
                .BatchUpdateAsync(c => new BlogComment { IsDeleted = true });
        }

        public async Task<bool> VoteAsync(int PostId, int CommentId, int UserId, bool? Existing, bool? ToBe)
        {
            if (Existing.HasValue != !ToBe.HasValue)
                throw new InvalidOperationException("Invalid vote status transition.");

            bool ok;

            if (ToBe == null)
            {
                ok = 1 == await Context.Set<BlogCommentVote>()
                    .Where(c => c.PostId == PostId && c.CommentId == CommentId && c.UserId == UserId && c.Up == Existing)
                    .BatchDeleteAsync();
            }
            else
            {
                ok = 1 == await Context.Set<BlogCommentVote>().UpsertAsync(
                    from c in Context.Set<BlogComment>()
                    where c.Id == CommentId && c.PostId == PostId
                    select new { c.PostId, CommentId = c.Id, UserId, ToBe.Value },
                    s => new BlogCommentVote
                    {
                        Up = s.Value,
                        CommentId = s.CommentId,
                        UserId = s.UserId,
                        PostId = s.PostId,
                    });
            }

            if (!ok) return false;

            (int up, int down) = (Existing, ToBe) switch
            {
                (true, null) => (-1, 0),
                (null, true) => (1, 0),
                (false, null) => (0, -1),
                (null, false) => (0, 1),
                _ => throw new InvalidOperationException(),
            };

            await Context.Set<BlogComment>()
                .Where(c => c.Id == CommentId)
                .BatchUpdateAsync(p => new BlogComment
                {
                    UpVotes = p.UpVotes + up,
                    DownVotes = p.DownVotes + down,
                });

            return true;
        }

        Task<Dictionary<int, BlogCommentVote>> ICommentStore.StatisticsAsync(int blogId, int userId)
        {
            return Context.Set<BlogCommentVote>()
                .Where(v => v.PostId == blogId && v.UserId == userId)
                .ToDictionaryAsync(k => k.CommentId);
        }

        public async Task<(BlogCommentVote, int, int)> StatisticsAsync(int blogId, int commentId, int userId)
        {
            var query =
                from c in Context.Set<BlogComment>()
                where c.Id == commentId && c.PostId == blogId
                join v in Context.Set<BlogCommentVote>() on new { c.Id, UserId = userId } equals new { Id = v.CommentId, v.UserId }
                into vv from v in vv.DefaultIfEmpty()
                select new { c.UpVotes, c.DownVotes, v };
            var result = await query.SingleOrDefaultAsync();
            return (result.v, result.UpVotes, result.DownVotes);
        }

        public Task<BlogComment> FindAsync(int postId, int commentId)
        {
            return Context.Set<BlogComment>()
                .Where(c => c.PostId == postId && c.Id == commentId)
                .SingleOrDefaultAsync();
        }
    }
}
