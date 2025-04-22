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

        [HttpGet]
        public IActionResult GetAllBooking()
        {
            var AllBooking = dbContext.Bookings.ToList();
            return Ok(AllBooking);
        }

        [HttpPost]
        public IActionResult CreateBooking(CreateBookingDto createBookingdto)
        {
            var BookingEntity = new Booking()
            {
                TouristId = createBookingdto.TouristId,
                TourPackageId = createBookingdto.TourPackageId,
                BookingDate = createBookingdto.BookingDate
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

        // GET: api/Booking/TrackStatus
        [HttpGet("TrackStatus")]
        public async Task<ActionResult<IEnumerable<BookingStatusDto>>> TrackBookingStatus([FromQuery] int touristId)
        {
            // Validate touristId
            var touristExists = await dbContext.Users.AnyAsync(t => t.UserID == touristId);
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
    }
    

}
