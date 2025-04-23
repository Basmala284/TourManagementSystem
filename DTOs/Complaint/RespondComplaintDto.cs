namespace TourManagementSystem.DTOs.Complaint
{
    public class RespondComplaintDto
    {
        public int ComplaintId { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // e.g., "Resolved", "Unresolved"
    }
}
