using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TourManagementSystem.DTOs.Users;
using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;
using TourManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using TourManagementSystem.DTOs;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration,
            JwtService jwtService,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("User is null");
            }

            var existingUser = await _userManager.FindByEmailAsync(userRegister.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            if (!Enum.TryParse(userRegister.Role, true, out Role userRole) || !Enum.IsDefined(typeof(Role), userRole))
            {
                return BadRequest("Invalid role specified. Allowed roles are: Admin, Tourist, TravelAgency.");
            }

            var newUser = new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.PhoneNumber,
                Role = userRole
            };

            var result = await _userManager.CreateAsync(newUser, userRegister.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // إنشاء الرول إذا لم تكن موجودة
            if (!await _roleManager.RoleExistsAsync(userRole.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(userRole.ToString()));
            }

            await _userManager.AddToRoleAsync(newUser, userRole.ToString());

            return Ok($"User registered successfully with role: {userRole}");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtService.GenerateJWTokenAsync(user.Id.ToString());

            var response = new
            {
                token = token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    role = roles.FirstOrDefault() // أو user.Role.ToString() إذا كنت تريد استخدام القيمة من الـ Enum مباشرة
                }
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return BadRequest("User not found");

            await _userManager.UpdateSecurityStampAsync(user);
            return Ok(new { message = "Logout successful" });
        }
    }
}