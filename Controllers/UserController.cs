using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.DTOs.Users;
using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext dbContext) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]        
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await dbContext.Users.AsNoTracking().Select(u => new UserResponseDto(u)).ToListAsync();
            return Ok(users);
        } 
        
        [Authorize(Roles = "Admin,Tourist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User not found.");


            return Ok(new UserResponseDto(user));
        }

        [Authorize(Roles = "Admin,Tourist")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, UserUpdateDto updatedUser)
        {
            var user = dbContext.Users.Find(id);
            if (user == null) return NotFound("User not found.");

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber; 

            dbContext.SaveChanges();
            return Ok("User updated successfully.");
        }

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
            if (changePasswordDto == null)
            {
                return BadRequest("Invalid request.");
            }

            var user = await dbContext.Users.FindAsync(changePasswordDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

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