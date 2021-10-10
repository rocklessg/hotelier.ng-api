using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.commons;
using hotel_booking_models;

namespace hotel_booking_utilities.AutoMapSetup
{
    public class UserMapInitializer : Profile
    {
        public UserMapInitializer()
        {
            // Authentication Maps
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
            CreateMap<Hotel, HotelBasicDto>();
            CreateMap<Manager, ManagersStatisticsDto>().ReverseMap();

            // Amenity Maps
            CreateMap<Amenity, AmenityDto>().ReverseMap();
        }

    }
}
