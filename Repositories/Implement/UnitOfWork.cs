using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityDbContext _dbContext;
        private IFacultyRepository _faultyRepository;
        private IMegazineRepository _megazineRepository;
        private IContributionRepository _contributionRepository;
        private IUserRepository _userRepository;
        private IImageRepository _imageRepository;

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
    }
}
