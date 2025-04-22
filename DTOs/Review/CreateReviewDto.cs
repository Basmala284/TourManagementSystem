using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.DTOs.Review
{
    public class CreateReviewDto
    {
        public string TourPackageTitle { get; set; }
        public int BookingID { get; set; }
        public int TourPackageId { get; set; }
        public int UserId { get; set; }


        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
