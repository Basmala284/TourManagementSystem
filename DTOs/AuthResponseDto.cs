namespace TourManagementSystem.DTOs
{
    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public IList<string> Roles { get; set; } = [];
    }
}
