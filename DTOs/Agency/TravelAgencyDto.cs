namespace TourManagementSystem.DTOs.Agency
{
    public class TravelAgencyDto
    {
        public int Id { get; set; }
        public string AgencyName { get; set; } // From User table
        public string AgencyEmail { get; set; } // From User table
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }
        public string Status { get; set; }
    }
}
