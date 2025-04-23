namespace TourManagementSystem.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int TouristId { get; set; }
        public int TourPackageId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }

    }
}
