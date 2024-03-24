﻿using DocumentFormat.OpenXml.Office2010.Excel;
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
                    Comment = c.Comments[0].CommentText,
                    CreatedCommentDate = c.Comments[0].CreatedDate,
                    FacultyUserName = c.Comments[0].UserId
                })
                .FirstOrDefaultAsync();

            return contribution;
        }

        public async Task<List<GetContributionModel>> SearchContribution(int megazineId, string? query)
        {
            IQueryable<Contribution> conQuery = _dbContext.Contributions;

            if (!string.IsNullOrEmpty(query))
            {
                conQuery = conQuery.Where(c => c.Title.Contains(query));
            }

            var contributions = await conQuery.Where(con => con.MegazineId == megazineId)
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