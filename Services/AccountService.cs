using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class AccountService : IAccountService
    {
        // Inject Main Repoistory 
        private readonly IMainRepoistory<User> _mainRepoistory;
        // Inject Account Repoistory
        private readonly IAccountRepoistory _accountRepoistory;
        // Inject Unit of Work
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IMainRepoistory<User> mainRepoistory, IUnitOfWork unitOfWork, IAccountRepoistory accountRepoistory )
        {
            _mainRepoistory = mainRepoistory;
            _unitOfWork = unitOfWork;
            _accountRepoistory = accountRepoistory;
        }

        // Delete user by ID
        public async Task<string> DeleteUserAsync( int userId )
        {
            var user = await _mainRepoistory.GetByIdAsync( userId );
            if (user == null)
                throw new Exception( "User not found." );

            await _mainRepoistory.DeleteAsync( userId );
            await _unitOfWork.SaveChangesAsync();

            return "User deleted successfully.";
        }


        // Get user by email
        public async Task<User> GetUserByEmailAsync( string email )
        {
            var user = await _accountRepoistory.GetUserByEmailAsync( email );
            if (user == null)
                throw new Exception( "User not found." );

            return user;

        }


        // Get user by ID
        public async Task<User> GetUserByIdAsync( int userId )
        {
            var user =await _mainRepoistory.GetByIdAsync( userId );
            if (user == null)
                throw new Exception( "User not found." );

            return user;
        }


        // Check if email is unique
        public async Task<bool> IsEmailUniqueAsync( string email )
        {
            var isUnique = await _accountRepoistory.IsEmailUniqueAsync( email );
            return isUnique;
        }


        // Login user
        public async Task<User> LoginUserAsync( LoginDto dto )
        {

            // validate user inputs
            if (string.IsNullOrEmpty( dto.Email ) || string.IsNullOrEmpty( dto.Password ))
            {
                throw new Exception( "Email and password are required." );
            }

            // find the user by email
            var user = await _accountRepoistory.GetUserByEmailAsync( dto.Email );

            if (user == null || !BCrypt.Net.BCrypt.Verify( dto.Password, user.PasswordHash ))
            {
                throw new Exception( "Invalid email or password." );
            }

            return user;


        }


        // Register user
        public async Task<User> RegisterUserAsync( RegisterDto dto )
        {
            var user = new User
            {
                FullName = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword( dto.Password ),
                Role = dto.Role
            };

            await _mainRepoistory.AddAsync( user );
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

       
    }
}
