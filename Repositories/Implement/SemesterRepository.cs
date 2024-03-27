using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        private UniversityDbContext _dbContext;
        public SemesterRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Faculties.AllAsync(x => x.Name == name);
        }
    }   
}
