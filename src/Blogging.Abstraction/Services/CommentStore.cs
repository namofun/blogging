using Blogging.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogging.Services
{
    public interface ICommentStore
    {
        Task<BlogComment> CreateAsync(BlogPost post, int uid, string contentMd, BlogComment replyTo = null);

        Task<BlogRevision> GetLastRevisionAsync(BlogComment comment);

        Task<List<BlogRevision>> GetRevisionsAsync(BlogComment comment);

        Task<BlogRevision> ReviseAsync(BlogComment comment, string rawContent);

        Task DeleteAsync(BlogComment comment);

        Task<BlogComment> FindAsync(int postId, int commentId);

        Task<bool> VoteAsync(int postId, int commentId, int userId, bool? existing, bool? toBe);

        Task<Dictionary<int, BlogCommentVote>> StatisticsAsync(int blogId, int userId);

        Task<(BlogCommentVote, int, int)> StatisticsAsync(int blogId, int commentId, int userId);
    }
}
