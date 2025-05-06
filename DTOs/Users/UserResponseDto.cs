using TourManagementSystem.Models.Entities;
using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.DTOs.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }

        public UserResponseDto(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            PhoneNumber = user.PhoneNumber;
        }
    }
}
