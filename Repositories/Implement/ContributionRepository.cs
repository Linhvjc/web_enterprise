using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Contribution;

namespace WebEnterprise.Repositories.Implement
{
    public class ContributionRepository : GenericRepository<Contribution>, IContributionRepository
    {
        private readonly UniversityDbContext _dbContext;
        public ContributionRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<List<GetContributionModel>> GetAllContributions(int id)
        {
            var contributions = await _dbContext.Contributions
                .Where(con => con.MegazineId == id)
                .Select(c => new GetContributionModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedDate = c.CreatedDate,
                    FullName = c.User.FullName,
                    ProfilePicture = c.User.ProfilePicture,
                    ReplyCount = c.Comments.Count()
                })
                .ToListAsync();

            return contributions;
        }

        public async Task<Contribution> GetContributionWithRelevant(int id)
        {
            var contribution = await _dbContext.Contributions
                .Include(d => d.User).Include(m => m.Megazine)
                .FirstOrDefaultAsync(c => c.Id == id);
            return contribution;
        }
    }
}
