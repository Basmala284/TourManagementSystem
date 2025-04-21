using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    [Table("Complaints")]
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }

        [ForeignKey("User")]
        public int TouristId { get; set; }
        public User Tourist { get; set; }
    }
}
