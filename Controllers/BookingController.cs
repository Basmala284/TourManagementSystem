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
    }
}
