using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourManagementSystem.Data;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public AgencyController(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }

    }
}
