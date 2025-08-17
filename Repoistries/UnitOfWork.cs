using E_Commerce.Data;
using E_Commerce.Repoistries.Base;

namespace E_Commerce.Repoistries
{
    public class UnitOfWork : IUnitOfWork
    {

        // Store repositories for each entity type to avoid creating them multiple times
        private readonly Dictionary<Type, object> _repositories = new();



        private readonly AppDbContext _context;
        public UnitOfWork( AppDbContext context )
        {
            _context = context;
        }
        // Commit changes to the database
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        // Repositories for different entities
        public IMainRepoistory<T> GetRepository<T>() where T : class
        {
            var type = typeof( T );

            // If repository for this entity doesn't exist, create and store it
            if (!_repositories.ContainsKey( type ))
            {
                var repoInstance = new MainRepoistory<T>( _context );
                _repositories[type] = repoInstance;
            }

            return (IMainRepoistory<T>)_repositories[type];
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
