namespace TourManagementSystem.DTOs.Review
{
    public class CreateReviewDto
    {
        public int BookingId { get; set; }
        public int TourPackageId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
