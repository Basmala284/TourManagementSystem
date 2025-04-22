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

        [HttpGet]
        public IActionResult GetCategories()
        {
            var Categories = dbContext.TripCategories.ToList();
            if (Categories == null)
            {
                return NotFound();
            }
            return Ok(Categories);
        }

        [HttpGet("{int id}")]
        public IActionResult CategorybyId(int id) {
            var Categoryid = dbContext.TripCategories.FirstOrDefault(c => c.Id == id);
            if (Categoryid == null)
            {
                return NotFound();
            }
            return Ok(Categoryid);


        }

        [HttpPut("{int id}")]
        public IActionResult UpdateCategory(int id,updateCategoryDto dto) {
            var category = dbContext.TripCategories.Find(id);
            if (category == null) return NotFound(" not found.");
           category.Name = dto.Name;
           category.Description = dto.Description;
            dbContext.SaveChanges();
            return Ok(category);

        }


        [HttpDelete("{int id}")]
        public IActionResult DeleteCategoryById(int id) {

            var _id = dbContext.TripCategories.Find(id);
            if (_id == null)
            {
                return NotFound();
            }
            dbContext.TripCategories.Remove(_id);
            dbContext.SaveChanges();
            return Ok(_id);
        }


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
            return Ok(category);

        }
    }
}