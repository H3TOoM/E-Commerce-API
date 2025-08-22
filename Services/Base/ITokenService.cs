using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user); 
    }
}
