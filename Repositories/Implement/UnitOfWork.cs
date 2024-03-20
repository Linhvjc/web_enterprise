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
        //private ISemesterRepository _semesterRepository;
        //private IFacultyRepository _faultyRepository;
        public IFacultyRepository FacultyRepository { get; }
        public ISemesterRepository SemesterRepository { get; }

        public UnitOfWork(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
            FacultyRepository = new FacultyRepository(_dbContext);
            
            SemesterRepository = new SemesterRepository(_dbContext);
        }

            //public IFacultyRepository FacultyRepository =>
            //    _faultyRepository ??= new FacultyRepository(_dbContext);
        public IMegazineRepository MegazineRepository =>
            _megazineRepository ??= new MegazineRepository(_dbContext);
        public IContributionRepository ContributionRepository =>
            _contributionRepository ??= new ContributionRepository(_dbContext);

        public IUserRepository UserRepository => 
            _userRepository ??= new UserRepository(_dbContext); 

        public IImageRepository ImageRepository =>
            _imageRepository ??= new ImageRepository(_dbContext);
        //public ISemesterRepository SemesterRepository =>
        //    _semesterRepository ??= new SemesterRepository(_dbContext);
    }
}
