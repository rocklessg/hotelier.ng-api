using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using System.Linq;

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


            CreateMap<Hotel, HotelBasicDto>();
            CreateMap<AppUser, UpdateAppUserDto>().ReverseMap();

            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();


            // Amenity Maps
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityRequestDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityResponseDto>().ReverseMap();
            CreateMap<Amenity, AmenityDto>().ReverseMap();


            // Hotel Maps
            CreateMap<Hotel, HotelBasicDto>()
                .ForMember(x => x.Thumbnail, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.Ratings.Sum(r => r.Ratings) / (double)src.Ratings.Count))
                .ForMember(x => x.NumberOfReviews, y => y.MapFrom(c => c.Ratings.Count));

            CreateMap<RoomType, RoomInfoDTo>()
                .ForMember(x => x.HotelName, y => y.MapFrom(c => c.Hotel.Name))
                .ForMember(x => x.DiscountPrice, y => y.MapFrom(c => c.Discount));

            CreateMap<GalleryDto, Gallery>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();

            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<AddHotelResponseDto, Hotel>().ReverseMap();
            CreateMap<Room, AddRoomDto>().ReverseMap();
            CreateMap<AddRoomResponseDto, Room>().ReverseMap();


        }

    }
}
