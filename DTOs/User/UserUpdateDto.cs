namespace TourManagementSystem.DTOs.User
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string resetPassword { get; set; }
        public int? PhoneNumber { get; set; }

    }
}
