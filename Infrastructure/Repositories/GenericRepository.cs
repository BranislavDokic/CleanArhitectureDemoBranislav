using Application.Interfaces.Repositoryinterfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepositoryInterface<T> where T : class
    {
        private readonly RealDatabase _realDatabase;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(RealDatabase realDatabase)
        {
            _realDatabase = realDatabase;
            _dbSet = _realDatabase.Set<T>();
        }
        
        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _realDatabase.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query); 
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<string> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            _dbSet.Remove(entity);
            await _realDatabase.SaveChangesAsync();
            return "Deleted";
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            _realDatabase.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _realDatabase.SaveChangesAsync();
            return existingEntity;
        }
    }
}
