using DocumentFormat.OpenXml.Office2010.Excel;
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
                    ReplyCount = c.Comments.Count(),
                    Megazine = c.Megazine.Name,
                })
                .ToListAsync();

            return contributions;
        }

        public async Task<List<GetContributionStudent>> GetAllContributionStudents(string userId)
        {
            var contributions = await _dbContext.Contributions.Where(c => c.UserId == userId)
                .Select(c => new GetContributionStudent
                {
                    Id = c.Id,
                    CreatedDate = c.CreatedDate,
                    MegazineName = c.Megazine.Name,
                    Status = c.Status,
                    Title = c.Title
                }).ToListAsync();

            return contributions;
        }

        public async Task<List<StatisticContribution>> GetContributionsWithout()
        {
            return await _dbContext.Contributions
                .Where(c => c.Comments.Count() == 0)
                .Select(c => new StatisticContribution
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedDate = c.CreatedDate,
                    MegazineName = c.Megazine.Name,
                    FacultyName = c.Megazine.Faculty.Name
                }).ToListAsync();
        }

        public async Task<List<StatisticContribution>> GetContributionsWithout14()
        {
           
            return await _dbContext.Contributions
                .Where(c => c.Comments.Count() == 0 && DateTime.UtcNow > c.CreatedDate.AddDays(14))
               .Select(c => new StatisticContribution
               {
                   Id = c.Id,
                   Title = c.Title,
                   CreatedDate = c.CreatedDate,
                   MegazineName = c.Megazine.Name,
                   FacultyName = c.Megazine.Faculty.Name
               }).ToListAsync();
        }

        public async Task<DetailContribution> GetContributionWithRelevant(int id)
        {

            var contribution = await _dbContext.Contributions
                .Where(c => c.Id == id)
                .Select(c => new DetailContribution
                {
                    Id = c.Id,
                    FullName = c.User.FullName,
                    CreatedDate = c.CreatedDate,
                    Title = c.Title,
                    ProfilePicture = c.User.ProfilePicture,
                    UserId = c.UserId,
                    FilePath = c.FilePath,
                    EndSemesterDate = c.Megazine.Semester.EndDate,
                    MegazineName = c.Megazine.Name,
                    FacultyName = c.Megazine.Faculty.Name,
                    FacultyId = c.Megazine.FacultyId,
                    numberContribution = _dbContext.Contributions.Count(u => u.UserId == c.UserId),
                    imagePaths = c.Images.Where(i => i.ContributionId == c.Id).Select(i => i.FilePath).ToList(),
                    Comment = c.Comments.Any() ? c.Comments[0].CommentText : null,
                    CreatedCommentDate = c.Comments.Any() ? c.Comments[0].CreatedDate : DateTime.MinValue,
                    FacultyUserName = c.Comments.Any() ? c.Comments[0].UserId : null
                })
                .FirstOrDefaultAsync();

            return contribution;
        }

        public async Task<List<GetContributionModel>> SearchContribution(int megazineId, string? query)
        {
            IQueryable<Contribution> conQuery = _dbContext.Contributions
                .Where(con => con.MegazineId == megazineId && con.Status == "Accept"); // Thêm điều kiện lọc theo Status

            if (!string.IsNullOrEmpty(query))
            {
                conQuery = conQuery.Where(c => c.Title.Contains(query));
            }

            var contributions = await conQuery
                .Select(c => new GetContributionModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedDate = c.CreatedDate,
                    FullName = c.User.FullName,
                    ProfilePicture = c.User.ProfilePicture,
                    ReplyCount = c.Comments.Count,
                    Megazine = c.Megazine.Name,
                    FilePath = c.FilePath,
                    Status = c.Status
                })
                .ToListAsync();

            return contributions;
        }

        public async Task<List<GetContributionModel>> SearchContribution(string semester)
        {
            IQueryable<Contribution> conQuery = _dbContext.Contributions;
            if (!string.IsNullOrEmpty(semester) && semester != "All")
            {
                conQuery = conQuery.Where(c => c.Megazine.Semester.Name == semester);
            }

            var contributions = await conQuery
                .Select(c => new GetContributionModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedDate = c.CreatedDate,
                    FullName = c.User.FullName,
                    ProfilePicture = c.User.ProfilePicture,
                    ReplyCount = c.Comments.Count(),
                    Megazine = c.Megazine.Name,
                    FilePath = c.FilePath,
                })
                .ToListAsync();

            return contributions;
        }
    }
}
