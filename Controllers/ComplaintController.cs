using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Complaint;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ComplaintController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Get All Complaints
        // GET: api/Complaints
        [HttpGet]
        public async Task<ActionResult> GetComplaints()
        {
            var complaints = await dbContext.Complaints.ToListAsync();
            return Ok(complaints);
        }

        // GET: api/Complaint/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetComplaintById(int id)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound("Complaint not found.");
            }
            return Ok(complaint);
        }
        //--> there is a problem in adding complaint<--
        // POST: api/Complaint
        [HttpPost]
        public async Task<ActionResult> AddComplaint(CreateComplaintDto complaintDto)
        {
            var complaint = new Complaint()
            {
                TouristId = complaintDto.TouristId,
                Title = complaintDto.Title,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                Response = ""
            };

            dbContext.Complaints.Add(complaint);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComplaints), new { id = complaint.Id }, complaint);
        }

        // PUT: api/Complaint/{id}/respond
        [HttpPut("{id}/respond")]
        public async Task<ActionResult> RespondToComplaint(int id, RespondComplaintDto responseDto)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            complaint.Status = "Resolved";
            complaint.Response = responseDto.Response;
            complaint.CreatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Complaint/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComplaint(int id)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }
            dbContext.Complaints.Remove(complaint);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
