using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    [Table("Reviews")]
    public class Review
    {
        [Key] public int Id { get; set; }

        public string TourPackageTitle { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        [ForeignKey("TripPackage")] public int TourPackageId { get; set; }
        public TripPackage TripPackage { get; set; }

        [ForeignKey("Booking")] public int BookingID { get; set; }

        public Booking Booking { get; set; }

    }
}
