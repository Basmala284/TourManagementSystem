namespace TourManagementSystem.DTOs.TripPackage
{
    public class CreateTripPackageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TripCategory { get; set; } // e.g., Adventure, Historical
        public string Destination { get; set; }
        public double Price { get; set; }
        public int DurationDays { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TravelAgencyId { get; set; }
    }
}
