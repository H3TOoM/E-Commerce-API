using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repoistries
{
    public class AddressRepoistory : IAddressRepoistory
    {
        // Inject the DbContext
        private readonly AppDbContext _context;
        public AddressRepoistory(AppDbContext context)
        {
            _context = context;
        }


        // Get all addresses for a specific user
        public Task<List<Address>> GetAddressesToUser( int userId )
        {
            var addresses = _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return addresses;
        }
    }
}
