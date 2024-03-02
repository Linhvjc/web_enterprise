namespace WebEnterprise.Repositories.Abstraction
{
    public interface IUnitOfWork
    {
        IFacultyRepository FacultyRepository { get; }
        IMegazineRepository MegazineRepository { get; }
    }
}
