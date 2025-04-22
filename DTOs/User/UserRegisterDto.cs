using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.DTOs.User
{
    public class UserRegisterDto
    {
        public string FullName { get; set; }
       
        public string Email { get; set; }
        public string Password{ get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public Role Role { get; set; }
    }
}
