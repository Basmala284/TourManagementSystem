using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.DTOs.User
{
    public class UserResponseDto
    {
        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public  Role Role { get; set; }
    }
}
