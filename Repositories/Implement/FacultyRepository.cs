using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Repositories.Implement
{
    public class FacultyRepository : GenericRepository<Faculty>, IFacultyRepository
    {
        private readonly UniversityDbContext _dbContext;
        public FacultyRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }

        public async Task<List<StatisticsFaculty>> GetStatisticsFaculties()
        {
                var statistics = await _dbContext.Megazines // Start with Megazines for a flat hierarchy
                     .Include(m => m.Contributions)
                         .ThenInclude(c => c.User)
                     .Include(m => m.Faculty)
                     .Include(m => m.Semester)
                     .GroupBy(m => new { SemesterName = m.Semester.Name, FacultyName = m.Faculty.Name })
                     .Select(g => new StatisticsFaculty
                     {
                         SemesterName = g.Key.SemesterName,
                         FacultyName = g.Key.FacultyName,
                         ContributionCount = g.SelectMany(m => m.Contributions).Count(), 
                         ContributorCount = g.SelectMany(m => m.Contributions)
                                                .Select(c => c.UserId).Distinct().Count()
                     }).ToListAsync();


            return statistics;
        }


        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Faculties.AllAsync(x => x.Name == name);
        }
    }
}
