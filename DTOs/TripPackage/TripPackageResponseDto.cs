namespace TourManagementSystem.DTOs.TripPackage
{
    public class TripPackageResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public int DurationDays { get; set; }
        public string TripCategory { get; set; }
        public string TravelAgencyName { get; set; }

    }
}
