using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourManagementSystem.Models.Entities
{
    public class TravelAgency
    {
        [Key]
        public int AgencyID { get; set; }    
        public bool IsApproved { get; set; }
        public string Address { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        public ICollection<TripPackage> TripPackages { get; set; }
    }
}
