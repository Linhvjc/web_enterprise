using Microsoft.EntityFrameworkCore;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UniversityDbContext _dbContext;
        public GenericRepository(UniversityDbContext businessDbContext)
        {
            _dbContext = businessDbContext;
        }
        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = GetById(id);
            return entity != null;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
