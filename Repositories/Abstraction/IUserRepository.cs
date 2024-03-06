namespace WebEnterprise.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<string> findCoorEmail(int facultyId);
    }
}
