using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Repoistries.Base
{
    public interface IAddressRepoistory
    {
        Task<List<Address>> GetAddressesToUser(int userId);
       
    }
}
