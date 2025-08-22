using System.Linq.Expressions;

namespace E_Commerce.Repoistries.Base
{
    public interface IMainRepoistory<T> where T : class
    {
        // CRUD operations
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllWithIncludeAsync( params Expression<Func<T, object>>[] includes );

        // Get All With Filter
        Task<IEnumerable<T>> GetAllWithFilterAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes );

        Task<T> GetByIdAsync( int id );
        Task<T> AddAsync( T entity );

        Task<T> UpdateAsync( int id, T entity );

        Task<string> DeleteAsync( int id );

        

    }
}
