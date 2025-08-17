using E_Commerce.Data;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Repoistries
{
    public class MainRepoistory<T> : IMainRepoistory<T> where T : class
    {
        // Inject the DbContext
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public MainRepoistory( AppDbContext context )
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();


        // Get all entities with included navigation properties
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync( params Expression<Func<T, object>>[] includes )
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include( include );
            }
            return await query.ToListAsync();
        }

        // Get entity by ID
        public async Task<T> GetByIdAsync( int id ) => await _dbSet.FindAsync( id );



        // Create a new entity
        public async Task<T> AddAsync( T entity )
        {
            await _dbSet.AddAsync( entity );
            return entity;
        }



        // Update an existing entity
        public async Task<T> UpdateAsync( int id, T entity )
        {
            if (id <= 0)
            {
                throw new ArgumentException( "Invalid ID provided." );
            }

            var existingEntity = await GetByIdAsync( id );
            if (existingEntity == null)
            {
                throw new KeyNotFoundException( "Entity not found." );
            }

            _context.Entry( existingEntity ).CurrentValues.SetValues( entity );
            return existingEntity;
        }


        // Delete an entity by ID
        public async Task<string> DeleteAsync( int id )
        {
            var entity = await GetByIdAsync( id );
            if (entity == null)
            {
                throw new KeyNotFoundException( "Entity not found." );
            }

            _dbSet.Remove( entity );
            return "Deleted successfully.";
        }


        // Get all entities with a filter and included navigation properties
        public async Task<IEnumerable<T>> GetAllWithFilterAsync( Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes )
        {

            IQueryable<T> query = _dbSet.Where( predicate );
            foreach (var include in includes)
            {
                query = query.Include( include );
            }
            return await query.ToListAsync();

        }
    }
}
