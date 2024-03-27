namespace WebEnterprise.Repositories.Abstraction
{
    public interface IUnitOfWork
    {
        IFacultyRepository FacultyRepository { get; }
        IMegazineRepository MegazineRepository { get; }
        IContributionRepository ContributionRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IUserRepository UserRepository { get; }
        IImageRepository ImageRepository { get; }
        ICommentRepository CommentRepository { get; }
    }
}
