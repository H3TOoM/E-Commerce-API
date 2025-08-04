using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {


        private readonly AppDbContext _context;
        public AddressController(AppDbContext context)
        {
            _context = context;
        }


        // Get the user ID from claims
        private int? GetUserIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (idClaim == null) return null;
            return int.Parse(idClaim.Value);
        }



        // Get all addresses for the user
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {

            var userId = GetUserIdFromClaims();
            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }

            // Fetch all addresses for the authenticated user
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
            if (addresses == null || !addresses.Any())
            {
                return NotFound();
            }
            return Ok(addresses);
        }


        // Add a new address
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto dto)
        {

            var userId = GetUserIdFromClaims();
            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized();
            }
            if (dto == null)
            {
                return BadRequest("Address data is required");
            }

            // Validate the address data
            var address = new Address
            {
                UserId = (int)userId,
                Country = dto.Country,
                City = dto.City,
                Street = dto.Street
            };

            // save the address to the database
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();


            // Return the created address
            return Ok(address);
        }


        // delete an address
        [Authorize]
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteAddress(int Id)
        {

            var userId = GetUserIdFromClaims();

            // Check if the user is authenticated
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == Id && a.UserId == userId);
            if (address == null)
            {
                return NotFound("Address not found or does not belong to the user");
            }
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return Ok("Address deleted successfully");
        }


        // Get my address
        [Authorize]
        [HttpGet("myAddresses")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var Addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Ok(Addresses);
        }
    }
}
