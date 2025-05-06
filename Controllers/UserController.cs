using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Users;
using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<User> userManager;

        public UserController(AppDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await dbContext.Users.AsNoTracking()
                .Select(u => new UserResponseDto(u))
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Roles = "Admin,Tourist")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User not found.");

            return Ok(new UserResponseDto(user));
        }

        [Authorize(Roles = "Admin,Tourist")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updatedUser)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null) return NotFound("User not found.");

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;

            await dbContext.SaveChangesAsync();
            return Ok("User updated successfully.");
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserRegisterDto userDto)
        {
            if (!Enum.TryParse(userDto.Role, true, out Role role))
            {
                return BadRequest("Invalid role specified");
            }

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = role
            };

            var result = await userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new UserResponseDto(user));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto == null)
            {
                return BadRequest(new { message = "Invalid request." });
            }

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                return BadRequest(new { message = "New password and confirmation don't match." });
            }

            var user = await userManager.FindByEmailAsync(changePasswordDto.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to change password.", errors = result.Errors });
            }

            return Ok(new { message = "Password changed successfully." });
        }
    }
}