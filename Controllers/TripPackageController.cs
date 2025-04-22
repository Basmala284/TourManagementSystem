using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.TripPackage;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripPackageController : ControllerBase
    {
        private readonly AppDbContext dbContext;

       

        public TripPackageController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            
        }



        // GET :api/TripPackage/ViewDetails
        // GET: api/TripPackage/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TripPackageDto>> GetTripDetails(int id)
        {
            // Fetch the trip package by ID
            var tripPackage = await dbContext.TripPackages
                .Include(tp => tp.TripCategory) // Include category details
                .FirstOrDefaultAsync(tp => tp.Id == id);

            // If the trip package is not found, return 404
            if (tripPackage == null)
            {
                return NotFound(new { message = "Trip package not found." });
            }

            // Map the entity to the DTO
            var tripDetailsDto = new TripPackageDto
            {
                Id = tripPackage.Id,
                Title = tripPackage.Title,
                Description = tripPackage.Description,
                Destination = tripPackage.Destination,
                Price = tripPackage.Price,
                DurationDays = tripPackage.DurationDays,
                StartDate = tripPackage.StartDate,
                EndDate = tripPackage.EndDate,
                AvailableSeats = tripPackage.AvailableSeats,
                Category = tripPackage.TripCategory.Description
            };

            // Return the trip details
            return Ok(tripDetailsDto);
        }





        // GET: api/TripPackage/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<TripPackageDto>>> SearchTrips(
            [FromQuery] string? category = null,
            [FromQuery] string? destination = null,
            [FromQuery] double? minPrice = null,
            [FromQuery] double? maxPrice = null,
            [FromQuery] int? minDuration = null,
            [FromQuery] int? maxDuration = null)
        {
            var query = dbContext.TripPackages.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(tp => tp.TripCategory.Description.Contains(category));
            }

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(tp => tp.Destination.Contains(destination));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(tp => tp.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(tp => tp.Price <= maxPrice.Value);
            }

            if (minDuration.HasValue)
            {
                query = query.Where(tp => tp.DurationDays >= minDuration.Value);
            }

            if (maxDuration.HasValue)
            {
                query = query.Where(tp => tp.DurationDays <= maxDuration.Value);
            }

            query = query.Where(tp => tp.AvailableSeats > 0); // Ensure trips have seats available

            var tripPackages = await query
                .Include(tp => tp.TripCategory) // Include category details if needed
                .ToListAsync();

            // Map to DTOs
            var tripPackageDtos = tripPackages.Select(tp => new TripPackageDto
            {
                Id = tp.Id,
                Title = tp.Title,
                Destination = tp.Destination,
                Price = tp.Price,
                DurationDays = tp.DurationDays,
                AvailableSeats = tp.AvailableSeats,
                Category = tp.TripCategory.Description
            }).ToList();

            return Ok(tripPackageDtos);
        }
    }
}
