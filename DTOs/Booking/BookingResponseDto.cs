namespace TourManagementSystem.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int TourPackageId { get; set; } 
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
    }
}
