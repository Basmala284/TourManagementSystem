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
        // GET: api/TravelAgency
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelAgencyDto>>> GetTravelAgencies()
        {
            // Fetch the agencies and include the related user's name and email
            var agencies = await dbContext.TravelAgencies
                .Include(a => a.User) // Include the related user
                .Select(a => new TravelAgencyDto
                {
                    Id = a.AgencyID,
                    AgencyName = a.User.FullName, // Name from User table
                    AgencyEmail = a.User.Email, // Email from User table
                    PhoneNumber = a.User.PhoneNumber,
                    IsApproved = a.IsApproved

                })
                .ToListAsync();

            return Ok(agencies);
        }



        // POST: api/Agency
        [HttpPost]
        public async Task<ActionResult> AddAgency(CreateAgencyDto agencyDto)
        {
            // Validate that the UserID exists in the User table
            var user = await dbContext.Users.FindAsync(agencyDto.UserID);
            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            // Ensure the User's name and email match the DTO values (optional validation)
            if (user.FullName != agencyDto.AgencyName || user.Email != agencyDto.AgencyEmail)
            {
                return BadRequest(new { message = "The provided AgencyName or AgencyEmail does not match the User." });
            }

            // Create the agency record
            var agency = new TravelAgency
            {
                UserID = agencyDto.UserID,
                IsApproved = false,

            };

            dbContext.TravelAgencies.Add(agency);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgencyById), new { id = agency.AgencyID }, agency);
        }

        // GET: api/Agency/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelAgencyDto>> GetAgencyById(int id)
        {
            // Fetch agency details, including user info
            var agency = await dbContext.TravelAgencies
                .Include(a => a.User)
                .Select(a => new TravelAgencyDto
                {
                    Id = a.AgencyID,
                    IsApproved = a.IsApproved,
                    Address = a.Address,
                    AgencyName = a.User.FullName,
                    AgencyEmail = a.User.Email
                })
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agency == null)
            {
                return NotFound(new { message = "Agency not found." });
            }

            return Ok(agency);
        }
        // PUT: api/TravelAgency/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveTravelAgency(int id)
        {
            var agency = await dbContext.TravelAgencies.FindAsync(id);
            if (agency == null)
            {
                return NotFound(new { message = "Travel Agency not found." });
            }

            agency.IsApproved = true;
            await dbContext.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // PUT: api/TravelAgency/{id}/reject
        [HttpPut("{id}/reject")]
        public async Task<ActionResult> RejectTravelAgency(int id)
        {
            var agency = await dbContext.TravelAgencies.FindAsync(id);
            if (agency == null)
            {
                return NotFound(new { message = "Travel Agency not found." });
            }

            agency.IsApproved = false;
            await dbContext.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // DELETE: api/TravelAgency/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTravelAgency(int id)
        {
            var agency = await dbContext.TravelAgencies.FindAsync(id);
            if (agency == null)
            {
                return NotFound(new { message = "Travel Agency not found." });
            }

            dbContext.TravelAgencies.Remove(agency);
            await dbContext.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}
