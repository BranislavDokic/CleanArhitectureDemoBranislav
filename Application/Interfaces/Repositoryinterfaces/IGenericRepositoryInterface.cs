

namespace Application.Interfaces.Repositoryinterfaces
{
    public interface IGenericRepositoryInterface<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<string> DeleteAsync(int id);
        Task<T> UpdateAsync(int id, T entity);
    }
}
