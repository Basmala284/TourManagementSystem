using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Agency;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public AgencyController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Agency

        [HttpGet]
        public async Task<ActionResult> GetAgencies()
        {
            var agencies = dbContext.TravelAgencies.ToList();
            return Ok(agencies);
        }

        // POST: api/TravelAgency
        [HttpPost]
        public async Task<ActionResult> AddTravelAgency(CreateAgencyDto agencyDto)
        {
            var agency = new TravelAgency()
            {
                FullName = agencyDto.FullName,
                Email = agencyDto.Email,
                Status = "Pending"
            };

            dbContext.TravelAgencies.Add(agency);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTravelAgencies), new { id = agency.Id }, agency);
        }

    }
}
