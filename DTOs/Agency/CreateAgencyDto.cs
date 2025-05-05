using System.ComponentModel.DataAnnotations;

namespace TourManagementSystem.DTOs.Agency
{
    public class CreateAgencyDto
    {

        public int UserID { get; set; }
        public string AgencyName { get; set; } // From User table
        public string AgencyEmail { get; set; } // From User table
        public bool IsApproved { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
