using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
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

        public Task<List<GetContributionModel>> GetAllContributions(int id)
        {
            var contributions = _dbContext.Contributions.Where(m => m.MegazineId == id)
                .Select(c => new GetContributionModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedDate = c.CreatedDate,
                    FullName = c.User.FullName,
                    ProfilePicture = c.User.ProfilePicture,
                    ReplyCount = c.Comments.Count()
                }).ToListAsync();

            return contributions;
        }
    }
}
