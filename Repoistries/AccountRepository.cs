using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class AccountRepository : IAccountRepoistory
    {

        // Injext AppDbContext
        private readonly AppDbContext _context;
        public AccountRepository( AppDbContext context )
        {
            _context = context;
        }



        public async Task<User> GetUserByEmailAsync( string email )
        {
            var user = await _context.Users.FirstOrDefaultAsync( u => u.Email == email );
            if (user == null)
                throw new Exception( "User not found." );
            return user;
        }



        public async Task<bool> IsEmailUniqueAsync( string email )
        {
            var isUnique = await _context.Users.AllAsync( u => u.Email != email );
            return isUnique;

        }

      

    }
}
