using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    public class TripCategory
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }    
        public string Description { get; set; }

        public List<TripPackage> TripPackages { get; set; }
    }
}
