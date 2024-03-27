using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Repositories.Abstraction;

namespace WebEnterprise.Repositories.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UniversityDbContext _dbContext;
        internal DbSet<T> dbSet;
        public GenericRepository(UniversityDbContext businessDbContext)
        {
            _dbContext = businessDbContext;
            this.dbSet = _dbContext.Set<T>();
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

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);

            // include properties will be comma separated
            if (!string.IsNullOrEmpty(includeProperties))
            {
                // apply the include properties
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll2(Expression<Func<T, bool>>? filter, string? includeProperties = null) // get all the properties
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // split the string by the comma
                {
                    query = query.Include(includeProp); // include the properties
                }
            }
            return query.ToList(); // return the list of the query
        }
        
    }
}
