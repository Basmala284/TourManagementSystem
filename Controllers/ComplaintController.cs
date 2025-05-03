using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        [Authorize(Roles = "Admin")]// Get all complaints (Admin-only)
        [HttpGet]
       
        public async Task<ActionResult> GetComplaints()
        {
            var complaints = await dbContext.Complaints.ToListAsync();
            if (complaints == null || !complaints.Any())
            {
                return NotFound(new { message = "No complaints found." });
            }
            return Ok(complaints);
        }

        [Authorize(Roles = "Admin")]  // Get a specific complaint by ID (Admin-only)
        [HttpGet("{id}")]
        
        public async Task<ActionResult> GetComplaintById(int id)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound(new { message = "Complaint not found." });
            }
            return Ok(complaint);
        }

        [Authorize(Roles = "Tourist")]   // Add a new complaint (Tourist-only)
        [HttpPost]
        
        public async Task<ActionResult> AddComplaint(CreateComplaintDto complaintDto)
        {
            // Ensure the logged-in user is the same as the TouristId in the DTO
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId != complaintDto.TouristId)
            {
                return Unauthorized(new { message = "You are not authorized to submit a complaint for another user." });
            }

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

            return CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, complaint);
        }

        [Authorize(Roles = "Admin")] // Respond to a complaint (Admin-only)
        [HttpPut("{id}/respond")]   
        public async Task<ActionResult> RespondToComplaint(int id, RespondComplaintDto responseDto)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound(new { message = "Complaint not found." });
            }

            complaint.Status = "Resolved";
            complaint.Response = responseDto.Response;
            complaint.CreatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Complaint responded to successfully.", complaint });
        }

        [Authorize(Roles = "Admin")] // Delete a complaint (Admin-only)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComplaint(int id)
        {
            var complaint = await dbContext.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound(new { message = "Complaint not found." });
            }

            dbContext.Complaints.Remove(complaint);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Complaint deleted successfully.", complaint });
        }
    }
}