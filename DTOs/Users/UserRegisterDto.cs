﻿using TourManagementSystem.Models.Enums;

namespace TourManagementSystem.DTOs.User
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
       
        public string Email { get; set; }
        public string Password{ get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
