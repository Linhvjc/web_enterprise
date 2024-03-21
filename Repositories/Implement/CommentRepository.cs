using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly UniversityDbContext _dbContext;
        public CommentRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<bool> IsCommented(int contributionId)
        {
            return await _dbContext.Comments.AnyAsync(c => c.ContributionId == contributionId);
        }
    }
}
