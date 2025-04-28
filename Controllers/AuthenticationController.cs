using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;
using TourManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;


namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _Token;
        private readonly SignInManager<User> _signInManager;


        public AuthenticationController(UserManager<User> userManager, TokenService Token, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration, SignInManager<User>? signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _Token = Token;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("User is null");
            }

            // Check if the user already exists
            var existingUser = await _userManager.FindByEmailAsync(userRegister.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            // Validate and parse the role
            Role userRole;

            // If Role is a string
            if (userRegister.Role is string roleString)
            {
                if (!Enum.TryParse(roleString, true, out userRole) || !Enum.IsDefined(typeof(Role), userRole))
                {
                    return BadRequest("Invalid role specified. Allowed roles are: Admin, Tourist, TravelAgency.");
                }
            }
     
            else
            {
                return BadRequest("Invalid role format.");
            }

            // Create a new user
            var newUser = new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.PhoneNumber,
                Role = userRole
            };

            // Create the user in the system
            var result = await _userManager.CreateAsync(newUser, userRegister.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add the user to the role in ASP.NET Identity
            await _userManager.AddToRoleAsync(newUser, userRole.ToString());

            return Ok($"User registered successfully with role: {userRole}");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("User is null");
            }

            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (await _userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                var userRole = user.Role.ToString(); // Get the role as a string

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiresInMinutes"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                        SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }

        //[Authorize]
        //[HttpPost("LogOut")]
        //public async Task<IActionResult> LogOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null) return BadRequest("not found");
        //    await _userManager.UpdateSecurityStampAsync(user);
        //    return Ok("log Out succesfully");

        //}
    }
}