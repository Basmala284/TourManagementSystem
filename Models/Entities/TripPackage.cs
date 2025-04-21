using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    [Table("TripPackages")]
    public class TripPackage
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public int DurationDays { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }


        [ForeignKey("TravelAgency")]
        public int TravelAgencyId { get; set; }


        // Foreign Key to TripCategory
        public int TripCategoryId { get; set; }
        public TripCategory TripCategory { get; set; }
        public TravelAgency TravelAgency { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Booking> Bookings { get; set; }

    }
}
