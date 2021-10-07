using AutoMapper;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;

namespace hotel_booking_utilities.AutoMapSetup
{
    public class MapInitializer : Profile
    {
        public MapInitializer()
        {
            // Authentication Maps
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, RegisterUserDto>().ReverseMap();
            CreateMap<AppUser, LoginDto>().ReverseMap();
<<<<<<< HEAD:hotel-booking-utilities/AutoMapSetup/UserMapInitializer.cs
            CreateMap<Hotel, HotelBasicDto>();
            CreateMap<AppUser, UpdateAppUserRequest>().ReverseMap();
=======
            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();

            // Hotel Maps
            CreateMap<Hotel, HotelBasicDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
>>>>>>> 5621354790f361918a3273e3d9b553446291e926:hotel-booking-utilities/AutoMapSetup/MapInitializer.cs
        }

    }
}
