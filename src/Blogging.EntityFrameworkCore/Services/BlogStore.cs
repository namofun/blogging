using Blogging.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public partial class BloggingFacade<TUser, TContext> : IBlogStore
    {
        public Task DeleteAsync(BlogPost post)
        {
            return Context.Set<BlogPost>()
                .Where(p => p.Id == post.Id)
                .BatchUpdateAsync(p => new BlogPost { IsDeleted = true });
        }

        public async Task<BlogPost> CreateAsync(int uid, string title, string rawContent, bool share)
        {
            var e = Context.Set<BlogPost>().Add(new BlogPost
            {
                PublishTime = DateTimeOffset.Now,
                UserId = uid,
                ContentHtml = "Preparing internally...",
            });

            await Context.SaveChangesAsync();
            await ReviseAsync(e.Entity, title, rawContent, share);
            return e.Entity;
        }

        public async Task<bool> SetCommonAsync(BlogPost post, bool common)
        {
            return 1 == await Context.Set<BlogPost>()
                .Where(p => p.Id == post.Id)
                .BatchUpdateAsync(p => new BlogPost { CommonShared = common });
        }

        public async Task<List<BlogPost>> ListAsync(int? byUser = null, int count = 10, int skip = 0)
        {
            var lst = Context.Set<BlogPost>()
                .Where(p => !p.IsDeleted);
            if (byUser.HasValue)
                lst = lst.Where(p => p.UserId == byUser);
            else
                lst = lst.Where(p => p.CommonShared);
            var lst2 =
                from p in lst
                join u in Context.Set<TUser>() on p.UserId equals u.Id
                select new { p, u.UserName, u.Email };
            var lst3 = await lst2
                .OrderByDescending(a => a.p.Id)
                .Skip(skip).Take(count)
                .ToListAsync();

            return lst3
                .Select(a =>
                {
                    var item = a.p;
                    item.UserDetail = (null, a.UserName, a.Email);
                    return item;
                })
                .ToList();
        }

        public async Task<BlogPost> FindAsync(int postId)
        {
            var post2Query =
                from p in Context.Set<BlogPost>()
                where p.Id == postId
                join u in Context.Set<TUser>() on p.UserId equals u.Id
                select new { p, u.UserName, u.Email };
            var post2 = await post2Query.SingleOrDefaultAsync();
            if (post2 == null) return null;
            var post = post2.p;
            post.UserDetail = (null, post2.UserName, post2.Email);

            post.Comments = new List<BlogComment>();
            var comment2Query =
                from c in Context.Set<BlogComment>()
                where c.PostId == postId
                join u in Context.Set<TUser>() on c.UserId equals u.Id
                select new { c, u.UserName, u.Email };
            var comments2 = await comment2Query.ToListAsync();
            var dict = comments2.ToDictionary(k => k.c.Id, k => k.c);

            foreach (var item2 in comments2)
            {
                var item = item2.c;
                item.UserDetail = (null, item2.UserName, item2.Email);
                if (item.ReplyToId == null)
                    post.Comments.Add(item);
                else
                    (dict[item.ReplyToId.Value].Replies ??= new List<BlogComment>()).Add(item);
            }

            return post;
        }

        public Task<BlogRevision> GetLastRevisionAsync(BlogPost post)
        {
            return Context.Set<BlogRevision>()
                .Where(r => r.BlogId == post.Id && r.Version == post.Revision)
                .SingleOrDefaultAsync();
        }

        public Task<List<BlogRevision>> GetRevisionsAsync(BlogPost post)
        {
            return Context.Set<BlogRevision>()
                .Where(r => r.BlogId == post.Id)
                .OrderBy(r => r.Version)
                .ToListAsync();
        }

        public async Task<BlogRevision> ReviseAsync(BlogPost post, string title, string rawContent, bool share)
        {
            var e = Context.Set<BlogRevision>().Add(new BlogRevision
            {
                BlogId = post.Id,
                ContentRaw = rawContent,
                Time = DateTimeOffset.Now,
                Version = post.Revision + 1
            });

            await Context.SaveChangesAsync();

            var result = Markdown.Render(rawContent, MarkdownPermission.None);
            post.Revision++;
            post.ContentHtml = result;
            post.Title = title;
            post.CommonShared = share;
            Context.Set<BlogPost>().Update(post);

            await Context.SaveChangesAsync();
            return e.Entity;
        }

        public async Task<bool> VoteAsync(int PostId, int UserId, bool? Existing, bool? ToBe)
        {
            if (Existing.HasValue != !ToBe.HasValue)
                throw new InvalidOperationException("Invalid vote status transition.");

            bool ok;

            if (ToBe == null)
            {
                ok = 1 == await Context.Set<BlogPostVote>()
                    .Where(c => c.PostId == PostId && c.UserId == UserId && c.Up == Existing)
                    .BatchDeleteAsync();
            }
            else
            {
                ok = 1 == await Context.Set<BlogPostVote>().UpsertAsync(
                    from c in Context.Set<BlogPost>()
                    where c.Id == PostId
                    select new { PostId = c.Id, UserId, ToBe.Value },

                    s => new BlogPostVote
                    {
                        Up = s.Value,
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

            await Context.Set<BlogPost>()
                .Where(c => c.Id == PostId)
                .BatchUpdateAsync(p => new BlogPost
                {
                    UpVotes = p.UpVotes + up,
                    DownVotes = p.DownVotes + down,
                });

            return true;
        }

        public Task<Dictionary<int, BlogPostVote>> StatisticsAsync(IEnumerable<int> blogIds, int userId)
        {
            return Context.Set<BlogPostVote>()
                .Where(v => blogIds.Contains(v.PostId) && v.UserId == userId)
                .ToDictionaryAsync(k => k.PostId);
        }

        async Task<(int, int, BlogPostVote)> IBlogStore.StatisticsAsync(int blogId, int userId)
        {
            var query =
                from c in Context.Set<BlogPost>()
                where c.Id == blogId
                join v in Context.Set<BlogPostVote>() on new { c.Id, UserId = userId } equals new { Id = v.PostId, v.UserId }
                into vv from v in vv.DefaultIfEmpty()
                select new { c.UpVotes, c.DownVotes, v };
            var result = await query.SingleOrDefaultAsync();
            return (result.UpVotes, result.DownVotes, result.v);
        }

        public Task<BlogPost> QuickFindAsync(int postId)
        {
            return Context.Set<BlogPost>()
                .Where(p => p.Id == postId)
                .SingleOrDefaultAsync();
        }
    }
}
