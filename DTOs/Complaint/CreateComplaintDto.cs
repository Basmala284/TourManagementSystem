namespace TourManagementSystem.DTOs.Complaint
{
    public class CreateComplaintDto
    {
        public int TouristId { get; set; }
        public string Title { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
