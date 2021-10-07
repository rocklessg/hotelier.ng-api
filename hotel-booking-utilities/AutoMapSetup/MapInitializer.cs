using AutoMapper;
using hotel_booking_dto.AmenityDtos;
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


            // Amenity Maps
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityRequestDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityResponseDto>().ReverseMap();

            // Hotel Maps
            CreateMap<Hotel, HotelBasicDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<AddHotelResponseDto, Hotel>().ReverseMap();
            CreateMap<Room, AddRoomDto>().ReverseMap();
            CreateMap<AddRoomResponseDto, Room>().ReverseMap();

        }

    }
}
