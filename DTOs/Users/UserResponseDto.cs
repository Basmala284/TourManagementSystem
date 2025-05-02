using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.DTOs.Users;

public class UserResponseDto (Models.Entities.User user)
{
    public int Id { get; set; } = user.Id;
    public string FullName { get; set; } = user.UserName;
    public string? Email { get; set; } = user.Email;
    public Role Role { get; set; } = user.Role;
    public string? PhoneNumber { get; set; } = user.PhoneNumber;
}
