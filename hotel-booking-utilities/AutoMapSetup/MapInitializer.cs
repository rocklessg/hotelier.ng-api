using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RoomDtos;
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
            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdateAppUserDto>().ReverseMap();


            // Amenity Maps
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityRequestDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityResponseDto>().ReverseMap();
            CreateMap<Amenity, AmenityDto>().ReverseMap();


            // Hotel Maps
            CreateMap<Hotel, HotelBasicDto>().ReverseMap();  
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelResponseDto>().ReverseMap();


            // Room Maps
            CreateMap<Room, AddRoomDto>().ReverseMap();
            CreateMap<Room, AddRoomResponseDto>().ReverseMap();


            // RoomType Maps
            CreateMap<RoomType, RoomInfoDto>().ReverseMap();


            // Gallery Maps
            CreateMap<Gallery, GalleryDto>().ReverseMap();

            //Customer
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

            // Transaction Maps
            CreateMap<Booking, TransactionResponseDto>()
                .ForMember(x => x.BookingId, y => y.MapFrom(s => s.Id))
                 .ForMember(x => x.HotelName, y => y.MapFrom(s => s.Hotel.Name))
                 .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.AppUser.FirstName + " " + s.Customer.AppUser.LastName));
        }

    }
}
