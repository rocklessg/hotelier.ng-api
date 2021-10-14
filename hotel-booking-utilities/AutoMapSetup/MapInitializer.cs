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
using System.Linq;
using hotel_booking_dto.ReviewDtos;

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
            CreateMap<Hotel, HotelBasicDto>()
                .ForMember(x => x.Thumbnail, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.PercentageRating, y => y.MapFrom(src => (double)src.Ratings.Sum(r => r.Ratings) * 100 / ((double)src.Ratings.Count * 5)))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.RoomTypes.OrderBy(rt => rt.Price).FirstOrDefault().Price));

            CreateMap<RoomType, RoomInfoDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(c => c.Hotel.Name))
                .ForMember(x => x.DiscountPrice, y => y.MapFrom(c => c.Discount));

            CreateMap<GalleryDto, Gallery>().ReverseMap();

            CreateMap<UpdateHotelDto, Hotel>().ReverseMap();

            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelResponseDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>()
                .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
                .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
                .ForMember(hotel => hotel.NumberOfReviews, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(hotel => hotel.Gallery, opt => opt.MapFrom(src => src.Galleries.Select(gallery => gallery.ImageUrl).ToList()));

            CreateMap<Hotel, GetAllHotelDto>()
               .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
               .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
               .ForMember(hotel => hotel.Gallery, opt => opt.MapFrom(src => src.Galleries.Select(gallery => gallery.ImageUrl).ToList()));

            

            // Room Maps
            CreateMap<Room, AddRoomDto>().ReverseMap();
            CreateMap<Room, AddRoomResponseDto>().ReverseMap();
            CreateMap<Room, RoomDTo>();


            // RoomType Maps
            CreateMap<RoomType, RoomInfoDto>().ReverseMap();
            CreateMap<RoomType, RoomTypeByHotelDTo>();
            CreateMap<RoomType, RoomTypeDto>();


            // Rating Maps
            CreateMap<Rating, HotelRatingsDTo>();

            // Gallery Maps
            CreateMap<Gallery, GalleryDto>().ReverseMap();
            //Customer
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

            // aminity
            CreateMap<Amenity, AmenityDto>();

            // reviewdto
            CreateMap<Review, ReviewDto>()
                .ForMember(review => review.CustomerImage, opt => opt.MapFrom(review => review.Customer.AppUser.Avatar))
                .ForMember(review => review.Text, opt => opt.MapFrom(review => review.Comment))
                .ForMember(review => review.Date, opt => opt.MapFrom(review => review.CreatedAt.ToShortDateString()));
        }

    }
}
