using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityDbContext _dbContext;

        private IMegazineRepository _megazineRepository;
        private IContributionRepository _contributionRepository;
        private IUserRepository _userRepository;
        private IImageRepository _imageRepository;
        private ISemesterRepository _semesterRepository;
        private IFacultyRepository _faultyRepository;

        private ICommentRepository _commentRepository;


        public UnitOfWork(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

            public IFacultyRepository FacultyRepository =>
                _faultyRepository ??= new FacultyRepository(_dbContext);
        public IMegazineRepository MegazineRepository =>
            _megazineRepository ??= new MegazineRepository(_dbContext);
        public IContributionRepository ContributionRepository =>
            _contributionRepository ??= new ContributionRepository(_dbContext);

        public IUserRepository UserRepository => 
            _userRepository ??= new UserRepository(_dbContext); 

        public IImageRepository ImageRepository =>
            _imageRepository ??= new ImageRepository(_dbContext);

        public ISemesterRepository SemesterRepository =>
            _semesterRepository ??= new SemesterRepository(_dbContext);


        public ICommentRepository CommentRepository =>
            _commentRepository ??= new CommentRepository(_dbContext);
    }
}
