using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Booking;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public BookingController(AppDbContext DbContext)
        {
            this.dbContext = DbContext;
        }


        // GetAllBooking  //get all booking for admin api

        [Authorize(Roles = "Admin,TravelAgency")]     // GetAllBooking (Admin or TravelAgency)
        [HttpGet]
        public IActionResult GetAllBooking()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Admin")
            {
                // Admin can see all bookings
                var allBookings = dbContext.Bookings
                    .Include(b => b.TripPackage)
                    .Include(b => b.Tourist)
                    .ToList();
                return Ok(allBookings);
            }
            else if (userRole == "TravelAgency")
            {
                // Travel Agency can only see bookings for their own packages
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var agencyPackages = dbContext.TripPackages
                    .Where(tp => tp.TravelAgencyId == userId)
                    .Select(tp => tp.Id)
                    .ToList();

                var agencyBookings = dbContext.Bookings
                    .Where(b => agencyPackages.Contains(b.TourPackageId))
                    .Include(b => b.TripPackage)
                    .Include(b => b.Tourist)
                    .ToList();
                return Ok(agencyBookings);
            }

            return Forbid();
        }


        //Add Booking (tourist)
        [Authorize(Roles = "Tourist")]
        [HttpPost]   
        public IActionResult CreateBooking(CreateBookingDto createBookingdto)
        {
            var BookingEntity = new Booking()
            {
                TouristId = createBookingdto.TouristId,
                TourPackageId = createBookingdto.TourPackageId,
                BookingDate = createBookingdto.BookingDate,
                Status = "pending"
            };
            dbContext.Bookings.Add(BookingEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Error saving changes: " + ex.InnerException?.Message);
                throw;
            }
            return Ok(BookingEntity);
        }

        // Get by Id (tourists)
        [Authorize(Roles = "Tourist")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponseDto>> GetBooking(int id)
        {
            var booking = await dbContext.Bookings
                .Include(b => b.TripPackage)
                .Include(b => b.Tourist)
                .ToListAsync();

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        // GET: api/Booking/TrackStatus (tourist)
        [Authorize(Roles = "Tourist")]
        [HttpGet("TrackStatus")]
        public async Task<ActionResult<IEnumerable<BookingStatusDto>>> TrackBookingStatus([FromQuery] int touristId)
        {
            // Validate touristId
            var touristExists = await dbContext.Users.AnyAsync(t => t.Id == touristId);
            if (!touristExists)
            {
                return NotFound(new { message = "Tourist not found" });
            }

            // Fetch bookings for the tourist
            var bookings = await dbContext.Bookings
                .Where(b => b.TouristId == touristId)
                .Include(b => b.TripPackage)
                .Select(b => new BookingStatusDto
                {
                    BookingId = b.Id,
                    TripPackageTitle = b.TripPackage.Title,
                    BookingDate = b.BookingDate,
                    Status = b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [Authorize(Roles = "TravelAgency")] // PUT: api/Booking/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveBooking(int id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                dbContext.SaveChanges();
                return NotFound();
            }

            booking.Status = "Approved";
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "TravelAgency")]    // PUT: api/Booking/{id}/reject
        [HttpPut("{id}/reject")]
        
        public async Task<ActionResult> RejectBooking(int id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }


            booking.Status = "Rejected";
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        // Cancel Booking (Tourist or Admin)
        [Authorize(Roles = "Tourist,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await dbContext.Bookings.Include(b => b.Tourist).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userRole == "Tourist" && booking.TouristId != userId)
            {
                // Tourist can only cancel their own bookings
                return Unauthorized(new { message = "You are not authorized to cancel this booking." });
            }

            // Admin or authorized Tourist can cancel the booking
            booking.Status = "Canceled";
            dbContext.Bookings.Update(booking);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
