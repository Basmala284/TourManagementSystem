using System.ComponentModel.DataAnnotations.Schema;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.DTOs.TripPackage
{
    public class UpdateTripPackageDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string TripCategory { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public int DurationDays { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        [ForeignKey("TravelAgency")]
        public int TravelAgencyId { get; set; }
        public TravelAgency TravelAgency { get; set; }
        public int AvailableSeat { get; set; }
    }
}
