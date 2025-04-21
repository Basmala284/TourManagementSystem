using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourManagementSystem.Models.Entities
{
    [Table("Messages")]
    public class Message

    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public int SenderId { get; set; }


        public int ReceiverId { get; set; }
    }
}
