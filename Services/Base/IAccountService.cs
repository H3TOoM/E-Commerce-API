using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface IAccountService
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<User> RegisterUserAsync(RegisterDto dto);
        Task<User> LoginUserAsync(LoginDto dto);

        Task<string> DeleteUserAsync(int userId);

        Task<User> GetUserByIdAsync(int userId);

        Task<User> GetUserByEmailAsync(string email);
    }
}
