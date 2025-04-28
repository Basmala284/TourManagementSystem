using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    public class TravelAgency
    {
        [Key]
        public int AgencyID { get; set; }    
        public bool IsApproved { get; set; }
        public string Address { get; set; }

        // Foreign key to User
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }


        public ICollection<TripPackage> TripPackages { get; set; }
    }
}
