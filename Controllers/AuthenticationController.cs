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
using TourManagementSystem.DTOs;


namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(UserManager<User> userManager, 
        RoleManager<IdentityRole<int>> roleManager, 
        IConfiguration configuration, 
        JwtService jwtService,
        SignInManager<User>? signInManager) : ControllerBase
    {
        private readonly SignInManager<User> _signInManager = signInManager;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                return BadRequest("User is null");
            }

            var existingUser = await userManager.FindByEmailAsync(userRegister.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            Role userRole;

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

            var newUser = new User
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.PhoneNumber,
                Role = userRole
            };

            var result = await userManager.CreateAsync(newUser, userRegister.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await userManager.AddToRoleAsync(newUser, userRole.ToString());

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

            var user = await userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (await userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                var authResponse = await jwtService.GenerateJWTokenAsync(user.Id.ToString());

                authResponse.Roles = roles;

                return Ok(authResponse);
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