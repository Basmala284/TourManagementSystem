using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.TripPackage;
using TourManagementSystem.Models.Entities;

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
        [Authorize(Roles = "Admin,TravelAgency,Tourist")]  // Get all trip packages (Admin, TravelAgency, or Tourist)
        [HttpGet]
        public IActionResult GetAllTripPackages()
        {
            var tripPackages = dbContext.TripPackages.AsNoTracking().ToList();
            return Ok(tripPackages);
        }


        [Authorize(Roles = "TravelAgency")]     // Create a new trip package
        [HttpPost]
        public IActionResult CreateTripPackage(CreateTripPackageDto request)
        {
            // Ensure the logged-in TravelAgency is creating the package
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId != request.TravelAgencyId)
            {
                return Unauthorized("You are not authorized to create a package for another agency.");
            }

            var tripPackage = new TripPackage()
            {
                TravelAgencyId = request.TravelAgencyId,
                Title = request.Title,
                Description = request.Description,
                Destination = request.Destination,
                Price = request.Price,
                DurationDays = request.DurationDays,
                AvailableSeats = request.AvailableSeats
            };

            dbContext.TripPackages.Add(tripPackage);
            dbContext.SaveChanges();
            return Ok("Trip package created successfully.");
        }

        [Authorize(Roles = "TravelAgency")] // Update trip package details (TravelAgency only)
        [HttpPut("{id}")]
        
        public IActionResult UpdateTripPackage(int id, UpdateTripPackageDto request)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");

            // Ensure the logged-in TravelAgency owns the package
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (tripPackage.TravelAgencyId != userId)
            {
                return Unauthorized("You are not authorized to update this trip package.");
            }

            tripPackage.Title = request.Title;
            tripPackage.Description = request.Description;
            tripPackage.Destination = request.Destination;
            tripPackage.Price = request.Price;
            tripPackage.DurationDays = request.DurationDays;
            tripPackage.AvailableSeats = request.AvailableSeat;

            dbContext.SaveChanges();
            return Ok("Trip package updated successfully.");
        }

        [Authorize(Roles = "Admin,TravelAgency")] // Delete trip package (Admin or TravelAgency)
        [HttpDelete("{id}")]
       
        public IActionResult DeleteTripPackage(int id)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");

            // Ensure the logged-in TravelAgency owns the package
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userRole == "TravelAgency" && tripPackage.TravelAgencyId != userId)
            {
                return Unauthorized("You are not authorized to delete this trip package.");
            }

            dbContext.TripPackages.Remove(tripPackage);
            dbContext.SaveChanges();
            return Ok("Trip package deleted successfully.");
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

       [Authorize(Roles = "Admin")] // Update trip package status Confirmed or not (Admin only)
       [HttpPut("{id}/status")]
        
        public IActionResult UpdateTripPackageStatus(int id, [FromQuery] string status)
        {
            var validStatuses = new List<string> { "Approved", "Rejected", "Pending" };

            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid status. Valid statuses are: Approved, Rejected, Pending.");
            }

            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null)
            {
                return NotFound("Trip package not found.");
            }

            tripPackage.Status = status;
            dbContext.SaveChanges();

            return Ok(new { tripPackage.Id, tripPackage.Status });
        }



        [Authorize(Roles = "Tourist")] // Search trips (Tourist only)
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

            query = query.Where(tp => tp.AvailableSeats > 0);

            var tripPackages = await query
                .Include(tp => tp.TripCategory)
                .ToListAsync();

            var tripPackageDtos = tripPackages.Select(tp => new TripPackageDto
            {
                Id = tp.Id,
                Title = tp.Title,
                Destination = tp.Destination,
                Price = tp.Price,
                DurationDays = tp.DurationDays,
                AvailableSeats = tp.AvailableSeats,
                Category = tp.TripCategory?.Description
            }).ToList();

            return Ok(tripPackageDtos);
        }


        [Authorize(Roles = "TravelAgency")] // Update trip availability (TravelAgency only)
        [HttpPut("{id}/availability")]
       
        public IActionResult UpdateTripAvailability(int id, [FromQuery] string status)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");

            // Ensure the logged-in TravelAgency owns the package
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (tripPackage.TravelAgencyId != userId)
            {
                return Unauthorized("You are not authorized to update the availability of this trip package.");
            }

            tripPackage.Status = status;
            dbContext.SaveChanges();
            return Ok(new { IsAvailable = status == "Available" });
        }
    }
}
