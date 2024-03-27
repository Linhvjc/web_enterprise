namespace WebEnterprise.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<string> findCoorEmail(int facultyId);
        Task<string> findStudentEmail(string userId);
        Task<bool> IsAllowed(string userId, int facultyId);
        Task<string> FindUserName(string userId);
    }
}
