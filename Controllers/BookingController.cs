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
        //get all booking for admin api
        [HttpGet]
        public IActionResult GetAllBooking()
        {
            var AllBooking = dbContext.Bookings.ToList();
            return Ok(AllBooking);
        }

        // get booking by id (tourist)
        [HttpGet]
        [Route("{id:int}")]

        public IActionResult GetBooking(int id)
        {

            var book = dbContext.Bookings.Find(id);

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);

        }

        [HttpGet("{Id}")]

        public async Task<ActionResult<BookingResponseDto>> GetBookingg(int Id)
        {
            var booking = await dbContext.Bookings
                .Include(b => b.TourPackageId)
                .Include(b => b.TouristId)
                .ToListAsync();

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking(CreateBookingDto createBookingdto)
        {
            var BookingEntity = new Booking()

            {
                TouristId = createBookingdto.TouristId,
                TourPackageId = createBookingdto.TourPackageId,
                BookingDate = createBookingdto.BookingDate,
                Status = "pending" //default status
            };

            dbContext.Bookings.Add(BookingEntity);
            dbContext.SaveChanges();

            return Ok(BookingEntity);
        }


        // PUT: api/Booking/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveBooking(int id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Approved";
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Booking/{id}/reject
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

        // DELETE: api/Booking/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            dbContext.Bookings.Remove(booking);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}
