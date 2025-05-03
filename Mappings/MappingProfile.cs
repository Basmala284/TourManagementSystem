using AutoMapper;
using TourManagementSystem.DTOs;
using TourManagementSystem.DTOs.Agency;
using TourManagementSystem.DTOs.Booking;
using TourManagementSystem.DTOs.Complaint;
using TourManagementSystem.DTOs.Message;
using TourManagementSystem.DTOs.Review;
using TourManagementSystem.DTOs.TripCategory;
using TourManagementSystem.DTOs.TripPackage;
using TourManagementSystem.DTOs.User;
using TourManagementSystem.DTOs.Users;
using TourManagementSystem.Models;
using TourManagementSystem.Models.Entities;

namespace TourManagementSystem.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // 🧍 USER
          
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<ChangePasswordDto, User>();
            CreateMap<UserResponseDto, User>();


            // 📦 TRIP PACKAGE
            CreateMap<TripPackage, TripPackageResponseDto>().ReverseMap();
            CreateMap<CreateTripPackageDto, TripPackage>();
            CreateMap<UpdateTripPackageDto, TripPackage>();
            CreateMap<TripPackageDto, TripPackage>();
            CreateMap<TripPackageResponseDto, TripPackage>();



            // 📝 REVIEW
            CreateMap<CreateReviewDto, Review>();

            // 📩 MESSAGE
            CreateMap<SendMessageDto, Message>();

            // ❗ COMPLAINT
            CreateMap<CreateComplaintDto, Complaint>();
            CreateMap<RespondComplaintDto, Complaint>();

            // 📅 BOOKING
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, BookingResponseDto>();
            CreateMap<BookingStatusDto, Booking>();


            // Category
            CreateMap<AddTripCategoryDto,TripCategory>();
            CreateMap<updateCategoryDto, TripCategory>();

            // Agency
            CreateMap<CreateAgencyDto,TravelAgency>();
            CreateMap<UpdateAgencyDto,TravelAgency>();
            CreateMap<TravelAgencyDto,TravelAgency>();


        }
    }
}
