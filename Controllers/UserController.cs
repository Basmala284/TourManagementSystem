using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourManagementSystem.Data;

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

        [HttpGet]

        public IActionResult GetAllUser()
        {
            var Users = dbContext.Users.ToList();
            return Ok(Users);
        }
    

    }
}
