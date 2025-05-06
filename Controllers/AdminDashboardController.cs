using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using System.Threading.Tasks;
using TourManagementSystem.DTOs.Agency;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]               // only admins can access this controller
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AdminDashboardController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/AdminDashboard/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = new
            {
                Agencies = await _dbContext.TravelAgencies.CountAsync(),
                Tours = await _dbContext.TripPackages.CountAsync(),
                Bookings = await _dbContext.Bookings.CountAsync(),
                Users = await _dbContext.Users.CountAsync()
            };

            return Ok(summary);
        }

        // GET: api/AdminNavbar/bookings
        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _dbContext.Bookings.ToListAsync();
            return Ok(bookings);
        }

        // GET: api/AdminNavbar/complaints
        [HttpGet("complaints")]
        public async Task<IActionResult> GetAllComplaints()
        {
            var complaints = await _dbContext.Complaints.ToListAsync();
            return Ok(complaints);
        }

        // GET: api/AdminNavbar/agencies
        [HttpGet("agencies")]
        public async Task<IActionResult> GetAllAgencies()
        {
            var agencies = await _dbContext.TravelAgencies.ToListAsync();
            return Ok(agencies);
        }

        // GET: api/AdminNavbar/trip-packages
        [HttpGet("trip-packages")]
        public async Task<IActionResult> GetAllTripPackages()
        {
            var tripPackages = await _dbContext.TripPackages.ToListAsync(); 
            return Ok(tripPackages);
        }


    }
}