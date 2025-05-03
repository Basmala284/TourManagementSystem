using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagementSystem.Data;
using TourManagementSystem.DTOs.TripCategory;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripCategoryController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TripCategoryController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]// Get all categories (Admin-only)
        [HttpGet]
        
        public IActionResult GetCategories()
        {
            var Categories = dbContext.TripCategories.ToList();
            if (Categories == null || !Categories.Any())
            {
                return NotFound(new { message = "No categories found." });
            }
            return Ok(Categories);
        }

        [Authorize(Roles = "Admin")]    // Get a category by ID (Admin-only)
        [HttpGet("{id}")]
        
        public IActionResult CategoryById(int id)
        {
            var category = dbContext.TripCategories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok(category);
        }

        // Update a category (Admin-only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]    
        public IActionResult UpdateCategory(int id, updateCategoryDto dto)
        {
            var category = dbContext.TripCategories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            dbContext.SaveChanges();
            return Ok(new { message = "Category updated successfully.", category });
        }

        [Authorize(Roles = "Admin")] // Delete a category by ID (Admin-only)
        [HttpDelete("{id}")]
       
        public IActionResult DeleteCategoryById(int id)
        {
            var category = dbContext.TripCategories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            dbContext.TripCategories.Remove(category);
            dbContext.SaveChanges();
            return Ok(new { message = "Category deleted successfully.", category });
        }

        [Authorize(Roles = "Admin")]   // Add a new category (Admin-only)
        [HttpPost]
        
        public IActionResult AddCategories(AddTripCategoryDto dto)
        {
            var category = new TripCategory
            {
                Name = dto.Name,
                Description = dto.Description
            };

            dbContext.TripCategories.Add(category);
            dbContext.SaveChanges();
            return Ok(new { message = "Category added successfully.", category });
        }
    }
}