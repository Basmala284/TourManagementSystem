namespace TourManagementSystem.DTOs.Booking
{
    public class BookingStatusDto
    {
        public int BookingId { get; set; }
        public string TripPackageTitle { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } // e.g., Pending, Confirmed, Canceled
    }
}
