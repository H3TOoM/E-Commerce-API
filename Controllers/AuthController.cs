using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {

            // check if the email already exists
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return BadRequest("Email already exists.");
            }

            // create a new user
            var user = new User
            {
                FullName = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            // add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            // return the token and user information
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email
                }
            });


        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }
            // generate JWT token
            var token = GenerateJwtToken(user);
            // return the token and user information
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.FullName,
                    user.Email
                }
            });
        }


        // Get Current user
        [Authorize]
        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> getCurrentUser()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            // check is user auth
            if (idClaim == null)
            {
                return Unauthorized("Invalid token. Id not found.");
            }

            int id = int.Parse(idClaim.Value);


            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found.");


            return Ok(user);

        }


        // GenerateJwtToken method to create a JWT token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "HatimAPI",
                audience: "HatimAPIClient",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
