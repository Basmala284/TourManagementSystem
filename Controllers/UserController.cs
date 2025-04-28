using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UserController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]        // Get all users For Admin only
        public IActionResult GetAllUsers()
        {
            var users = dbContext.Users
                .Select(user => new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.Role
                })
                .ToList();

            return Ok(users);
        } 
        
        // Get user by ID
        [Authorize(Roles = "Admin,Tourist")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.Role
                })
                .FirstOrDefault();

            if (user == null) return NotFound("User not found.");

            return Ok(user);
        }

        // Update user
        [Authorize(Roles = "Admin,Tourist")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, UserUpdateDto updatedUser)
        {
            var user = dbContext.Users.Find(id);
            if (user == null) return NotFound("User not found.");

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber; // Optional, for example only

            dbContext.SaveChanges();
            return Ok("User updated successfully.");
        }

        // Add new user
        [HttpPost]
        public async Task<ActionResult> AddUser(UserRegisterDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = userDto.Password,
                PhoneNumber = userDto.PhoneNumber,
                Role = Role.Tourist,
              
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Validate input
            if (changePasswordDto == null)
            {
                return BadRequest("Invalid request.");
            }

            // Find the user by ID
            var user = await dbContext.Users.FindAsync(changePasswordDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Use UserManager to change the password
            var userManager = HttpContext.RequestServices.GetService<UserManager<User>>();
            if (userManager == null)
            {
                return StatusCode(500, "UserManager service is not available.");
            }

            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully.");
        }
    }
}