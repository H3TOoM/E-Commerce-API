using E_Commerce.DTOs;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // inject Service
        private readonly IAccountService _accountService;

        // Inject Token Service
        private readonly ITokenService _tokenService;

        public AccountController( IAccountService accountService, ITokenService tokenService )
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }



        // Register User
        [HttpPost( "register" )]
        public async Task<IActionResult> Register( RegisterDto dto )
        {
            try
            {
                var isEmailUnique = await _accountService.IsEmailUniqueAsync( dto.Email );
                if (!isEmailUnique)
                {
                    return BadRequest( "Email is already in use." );
                }

                var user = await _accountService.RegisterUserAsync( dto );
                var token = await _tokenService.GenerateTokenAsync( user );

                return Ok( new { Token = token, User = user } );

            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex.Message}" );
            }
        }


        // Login User
        [HttpPost( "login" )]
        public async Task<IActionResult> Login( LoginDto dto )
        {
            try
            {
                var user = await _accountService.GetUserByEmailAsync( dto.Email );
                if (user == null)
                {
                    return Unauthorized( "Invalid email or password." );
                }

                var loggedInUser = await _accountService.LoginUserAsync( dto );
                var token = await _tokenService.GenerateTokenAsync( loggedInUser );

                return Ok( new { Token = token, User = loggedInUser } );
            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex.Message}" );
            }

        }

        // Get User by ID
        [Authorize]
        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetUserById( int id )
        {
            try
            {
                var user = await _accountService.GetUserByIdAsync( id );
                if (user == null)
                {
                    return NotFound( "User not found." );
                }
                return Ok( user );
            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex.Message}" );
            }
        }

        [Authorize]
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteUser( int id )
        {
            try
            {
                var result = await _accountService.DeleteUserAsync( id );
                return Ok( result );
            }
            catch (Exception ex)
            {
                return StatusCode( 500, $"Internal server error: {ex.Message}" );
            }
        }



    }
}
