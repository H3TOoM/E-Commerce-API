using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface IAccountRepoistory
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<User> GetUserByEmailAsync(string email); 
    }
}
