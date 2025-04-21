using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourManagementSystem.Models.Entities
{
    [Table("Bookings")]
    public class Booking
    { 
        [Key]
        public int Id { get; set; }

        public DateTime BookingDate { get; set; }
        public string Status { get; set; }              // Enum: Pending, Confirmed, Cancelled, etc

        [ForeignKey("TripPackage")]
        public int TourPackageId { get; set; }
        public TripPackage TripPackage { get; set; }     

        [ForeignKey("User")]
        public int TouristId { get; set; }
        public User Tourist { get; set; }


        public Review Review { get; internal set; }

   
    }
}
