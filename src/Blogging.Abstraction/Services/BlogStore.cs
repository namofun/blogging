using Blogging.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public interface IBlogStore
    {
        Task<BlogPost> CreateAsync(int uid, string title, string rawContent, bool share);

        Task<bool> SetCommonAsync(BlogPost post, bool common);

        Task<List<BlogPost>> ListAsync(int? byUser = null, int count = 10, int skip = 0);

        Task<BlogPost> FindAsync(int postId);

        Task<BlogPost> QuickFindAsync(int postId);

        Task<BlogRevision> GetLastRevisionAsync(BlogPost post);

        Task<List<BlogRevision>> GetRevisionsAsync(BlogPost post);

        Task<BlogRevision> ReviseAsync(BlogPost post, string title, string rawContent, bool share);

        Task DeleteAsync(BlogPost post);

        Task<bool> VoteAsync(int postId, int userId, bool? existing, bool? toBe);

        Task<Dictionary<int, BlogPostVote>> StatisticsAsync(IEnumerable<int> blogIds, int userId);

        Task<(int, int, BlogPostVote)> StatisticsAsync(int blogIds, int userId);
    }
}
