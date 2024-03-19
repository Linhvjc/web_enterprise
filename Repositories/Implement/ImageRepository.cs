using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.Models.Entities;
using WebEnterprise.Infrastructure.Persistance;

namespace WebEnterprise.Repositories.Implement
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly UniversityDbContext _dbContext;
        public ImageRepository(UniversityDbContext businessDbContext) : base(businessDbContext)
        {
            _dbContext = businessDbContext;
        }
    }
}
