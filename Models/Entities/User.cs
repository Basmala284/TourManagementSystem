using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TourManagementSystem.Models.Enums;
namespace TourManagementSystem.Models.Entities
{
    [Table("Users")]
    public class User : IdentityUser<int>
    {
        // Map Identity's "Id" to "UserID"
        [Key]
        [Column("UserID")] // the column is named "UserID" in the database
        public override int Id { get; set; } // This will act as UserID

        public Role Role { get; set; }
        public string UserName { get; set; }

        //public string Email { get; set; }
        //public string Password { get; set; }
        // public string resetPassword { get; set; }
        //public String PhoneNumber { get; set; }
       
        public List<TravelAgency> TravelAgencies { get; set; }
        public List<Complaint> Complaints { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
