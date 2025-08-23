using E_Commerce.DTOs;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private readonly IAddressService _addressService;
        public AddressController( IAddressService addressService )
        {
            _addressService = addressService;
        }



        // Get the user ID from claims
        private int getUserIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier );
            if (idClaim == null)
            {
                throw new UnauthorizedAccessException( "User not authenticated" );
            }
            return int.Parse( idClaim.Value );

        }



        // Get addresses by user id
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = getUserIdFromClaims();
            var addresses = await _addressService.GetAddressesToUser( userId );
            if (!addresses.Any())
                return NotFound( "No addresses found for the user." );

            return Ok( addresses );
        }


        // Get address by id
        [Authorize]
        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetAddressById( int id )
        {
            var userId = getUserIdFromClaims();
            var address = await _addressService.GetAddressById( id );
            if (address == null)
            {
                return NotFound( "Address not found." );
            }
            return Ok( address );
        }


        // Add new address
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAddress( AddressDto dto )
        {
            try
            {
                var userId = getUserIdFromClaims();
                var address = await _addressService.AddAddress( dto );
                return CreatedAtAction( nameof( GetAddresses ), new { id = address.Id }, address );
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }

        // Update address
        [Authorize]
        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateAddress( int id, AddressDto dto )
        {
            try
            {
                var userId = getUserIdFromClaims();
                var address = await _addressService.UpdateAddress( id, dto );
                if (address == null)
                {
                    return NotFound( "Address not found." );
                }
                return Ok( address );
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }


        // Delete address
        [Authorize]
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteAddress( int id )
        {
            try
            {
                var userId = getUserIdFromClaims();
                var success = await _addressService.DeleteAddress( id );
                if (!success)
                {
                    return NotFound( "Address not found." );
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message );
            }
        }



    }
}
