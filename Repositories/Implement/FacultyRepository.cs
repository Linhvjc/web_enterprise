using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class FacultyRepository : GenericRepository<Faculty>, IFacultyRepository
    {
        private readonly UniversityDbContext _dbContext;
        public FacultyRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Faculties.AllAsync(x => x.Name == name);
        }
    }
}
