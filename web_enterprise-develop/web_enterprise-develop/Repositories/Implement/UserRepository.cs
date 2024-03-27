using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversityDbContext _dbContext;
        public UserRepository(UniversityDbContext businessDbContext)
        {
            _dbContext = businessDbContext;
        }
        public Task<string> findCoorEmail(int facultyId)
        {
            var email = _dbContext.Users.Where(u => u.FacultyId == facultyId)
                .Join(_dbContext.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new {User = u, UserRoleId = ur.RoleId})
                .Join(_dbContext.Roles,
                ur => ur.UserRoleId,
                r => r.Id,
                (ur, r) => new { User = ur.User, Role = r })
                .Where(x => x.Role.Name == "COORDINATOR")
                .Select(x => x.User.Email)
                .FirstOrDefaultAsync();
            return email;
        }

        public async Task<string> findStudentEmail(string userId)
        {
            return await _dbContext.Users.Where(u => u.Id == userId).Select(x => x.Email).FirstOrDefaultAsync();
        }

        public async Task<string> FindUserName(string userId)
        {
            return await _dbContext.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefaultAsync();
        }

        public async Task<bool> IsAllowed(string userId, int facultyId)
        {
            var userCoId = _dbContext.Users.Where(u => u.FacultyId == facultyId)
                .Join(_dbContext.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new { User = u, UserRoleId = ur.RoleId })
                .Join(_dbContext.Roles,
                ur => ur.UserRoleId,
                r => r.Id,
                (ur, r) => new { User = ur.User, Role = r })
                .Where(x => x.Role.Name == "COORDINATOR")
                .Select(x => x.User.Id)
                .FirstOrDefaultAsync();
            return await userCoId == userId;
        }
    }
}
