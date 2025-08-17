namespace E_Commerce.Repoistries.Base
{
    public interface IUnitOfWork : IDisposable
    {


        // Commit changes to the database
        Task<int> SaveChangesAsync();

        // Repositories for different entities
        IMainRepoistory<T> GetRepository<T>() where T : class;
       
    }
}
