using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllReviews()
        {
            var reviews = dbContext.Reviews
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    TourPackageTitle = r.TourPackageTitle,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                })
                .ToList();

            if (reviews == null || !reviews.Any())
            {
                return NotFound(new { message = "No reviews found." });
            }

            return Ok(reviews);
        }
        [Authorize(Roles = "Tourist")] // Add a review (Tourist only)
        [HttpPost]
        public IActionResult AddReview(CreateReviewDto request)
        {
            // Ensure the logged-in user is the same as the Tourist in the Booking
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var booking = dbContext.Bookings
                .Include(b => b.Tourist) 
                .Include(b => b.TripPackage) 
                .FirstOrDefault(b => b.Id == request.BookingID);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found for the given BookingId." });
            }

            if (booking.TouristId != userId)
            {
                return Unauthorized(new { message = "You are not authorized to add a review for this booking." });
            }

            // Check if a review already exists for this booking
            if (booking.Review != null)
            {
                return BadRequest(new { message = "A review has already been submitted for this booking." });
            }


            var review = new Review()
            {
                BookingID = booking.Id, 
                TourPackageId = booking.TourPackageId, 
                TourPackageTitle = booking.TripPackage.Title, 
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow 
            };

            dbContext.Reviews.Add(review);

            booking.Review = review;
            dbContext.SaveChanges();

            return Ok(new { message = "Review added successfully.", review });
        }

        [Authorize(Roles = "Admin")]// Delete a review (Admin-only)
        [HttpDelete("{id}")]
        
        public async Task<ActionResult> DeleteReview(int id)
        {
            var review = await dbContext.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound(new { message = "Review not found." });
            }

            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Review deleted successfully." });
        }
    }
}