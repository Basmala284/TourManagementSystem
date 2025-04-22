namespace TourManagementSystem.DTOs.TripPackage
{
    public class TripPackageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int DurationDays { get; set; }
        public int AvailableSeats { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Category { get; set; }
    }
}
