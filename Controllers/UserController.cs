using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.Models.Entities;

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

        //Get All users Api For Admin
        [HttpGet]

        public IActionResult GetAllUser()
        {
            var Users = dbContext.Users.ToList();
            return Ok(Users);
        }

        // Get user by id Api For Admin
        [HttpGet("{id}")]

        public IActionResult GetUserById(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        // update userinfo by id 
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserUpdateDto updatedUser)
        {
            var user = dbContext.Users.Find(id);
            if (user == null) return NotFound("User not found.");

            user.FullName = updatedUser.FullName;
            user.Password = updatedUser.Password;
            user.resetPassword = updatedUser.resetPassword;
            user.Email = updatedUser.Email;

            dbContext.SaveChanges();
            return Ok("User updated successfully.");
        }

        // POST: api/User/
        //Add new user to system
        [HttpPost]
        public async Task<ActionResult> AddUser(UserRegisterDto userDto)
        {
            var user = new User
            {
                FullName = userDto.FullName,
                Email = userDto.Email,
                Role = userDto.Role,
                Password = userDto.Password ,
                PhoneNumber = userDto.PhoneNumber,
                Gender = userDto.Gender,
               resetPassword = ""
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return Ok(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

    }
}
