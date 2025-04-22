using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.Review;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ReviewController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: api/Review
        [HttpGet]
        public IActionResult GetAllReviews()
        {
            var reviews = dbContext.Reviews.ToList();
            return Ok(reviews);
        }

        // Add review (tourist)
        [HttpPost]
        public IActionResult AddReview(CreateReviewDto request)
        {
            // Fetch the Booking record to ensure it exists and fetch related data
            var booking = dbContext.Bookings
                .Include(b => b.Tourist) // Ensure the Tourist navigation property is loaded
                .Include(b => b.TripPackage) // Ensure the TripPackage navigation property is loaded
                .FirstOrDefault(b => b.Id == request.BookingID);

            if (booking == null)
            {
                return NotFound("Booking not found for the given BookingId.");
            }

            // Check if a review already exists for this booking
            if (booking.Review != null)
            {
                return BadRequest("A review has already been submitted for this booking.");
            }

            // Create the Review object
            var review = new Review()
            {
                BookingID = booking.Id, // Associate the review with the booking
               // TouristId = booking.TouristId, // Get the UserId from the Booking
                TourPackageId = booking.TourPackageId, // Get the TourPackageId from the Booking
                TourPackageTitle = booking.TripPackage.Title, // Get the Title from the TripPackage
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow // Set the review date to the current time
            };

            // Add the review to the context
            dbContext.Reviews.Add(review);

            // Update the Booking entity to associate it with the Review
            booking.Review = review;

            // Save changes to the database
            dbContext.SaveChanges();

            return Ok(review);
        }
        // DELETE: api/Review/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var review = await dbContext.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}