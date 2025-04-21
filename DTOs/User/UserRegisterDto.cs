namespace TourManagementSystem.DTOs.User
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string Email { get; set; }
        public string Password{ get; set; }
        public string Gender { get; set; }
        public int PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
    }
}
