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


        [HttpGet]
        public IActionResult GetAllTripPackages()
        {
            var tripPackages = dbContext.TripPackages.ToList();
            return Ok(tripPackages);
        }

        [HttpGet("{id}")]
        public IActionResult GetTripPackageById(int id)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");
            return Ok(tripPackage);
        }

        [HttpPost]
        public IActionResult CreateTripPackage(CreateTripPackageDto request)
        {
            var tripPackage = new TripPackage()
            {
                TravelAgencyId = request.TravelAgencyId,
                Title = request.Title,
                Description = request.Description,
                Destination = request.Destination,
                Price = request.Price,
                DurationDays = request.DurationDays,
                AvailableSeat = request.AvailableSeat
            };

            dbContext.TripPackages.Add(tripPackage);
            dbContext.SaveChanges();
            return Ok("Trip package created successfully.");
        }

        [HttpPut("{id}")]
        
        public IActionResult UpdateTripPackage(int id,UpdateTripPackageDto request)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");

            tripPackage.Title = request.Title;
            tripPackage.Description = request.Description;
            
            tripPackage.Destination = request.Destination;
            tripPackage.Price = request.Price;
            tripPackage.DurationDays = request.DurationDays;
            tripPackage.AvailableSeat = request.AvailableSeat;

            dbContext.SaveChanges();
            return Ok("Trip package updated successfully.");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTripPackage(int id)
        {
            var tripPackage = dbContext.TripPackages.Find(id);
            if (tripPackage == null) return NotFound("Trip package not found.");

            dbContext.TripPackages.Remove(tripPackage);
            dbContext.SaveChanges();
            return Ok("Trip package deleted successfully.");
        }


    }
}
