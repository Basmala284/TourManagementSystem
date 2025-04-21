using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TourManagementSystem.Models.Enums;
namespace TourManagementSystem.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
         public string resetPassword { get; set; }

        public Role Role { get; set; }

        public String PhoneNumber { get; set; }
        public int Age { get; set; }
        public List<TravelAgency> TravelAgencies { get; set; }
        public List<Complaint> Complaints { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
