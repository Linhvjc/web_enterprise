using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class MegazineRepository : GenericRepository<Megazine>, IMegazineRepository
    {
        private readonly UniversityDbContext _dbContext;
        public MegazineRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<List<Megazine>> GetMegazinesWithRelevant()
        {
            var megazines = await _dbContext.Megazines.Include(f => f.Faculty).ToListAsync();
            return megazines;
        }
    }
}
