using AutoMapper;
using TourManagementSystem.DTOs;
using TourManagementSystem.DTOs.Booking;
using TourManagementSystem.DTOs.Complaint;
using TourManagementSystem.DTOs.Message;
using TourManagementSystem.DTOs.Review;
using TourManagementSystem.DTOs.TripPackage;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.Models;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // 🧍 USER
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserLoginDto, User>();

            // 📦 TRIP PACKAGE
            CreateMap<TripPackage, TripPackageResponseDto>().ReverseMap();
            CreateMap<CreateTripPackageDto, TripPackage>();
            CreateMap<UpdateTripPackageDto, TripPackage>();

            // 📝 REVIEW
            CreateMap<CreateReviewDto, Review>();

            // 📩 MESSAGE
            CreateMap<SendMessageDto, Message>();

            // ❗ COMPLAINT
            CreateMap<CreateComplaintDto, Complaint>();

            // 📅 BOOKING
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, BookingResponseDto>();
        }
    }
}
