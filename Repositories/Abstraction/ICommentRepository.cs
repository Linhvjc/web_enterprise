

using WebEnterprise.Models.Entities;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<bool> IsCommented(int contributionId);
    }
}
