using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_dto.RoomDtos;
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
            CreateMap<AppUser, ResetPasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdatePasswordDto>().ReverseMap();
            CreateMap<AppUser, UpdateAppUserDto>().ReverseMap();

            // Amenity Maps
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityRequestDto>().ReverseMap();
            CreateMap<Amenity, AddAmenityResponseDto>().ReverseMap();
            CreateMap<Amenity, AmenityDto>().ReverseMap();


            // Hotel Maps
            CreateMap<Hotel, HotelBasicDetailsDto>()
                .ForMember(x => x.Thumbnail, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.PercentageRating, y => y.MapFrom(src => src.Ratings.Count == 0 ? 100 : (double)src.Ratings.Sum(r => r.Ratings) * 100 / ((double)src.Ratings.Count * 5)))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.RoomTypes.OrderBy(rt => rt.Price).FirstOrDefault().Price));


            CreateMap<RoomType, RoomInfoDto>()
                .ForMember(x => x.HotelName, y => y.MapFrom(c => c.Hotel.Name))
                .ForMember(x => x.DiscountPrice, y => y.MapFrom(c => c.Discount));

            CreateMap<UpdateHotelDto, Hotel>().ReverseMap();

            CreateMap<Hotel, HotelBasicDto>() 
                .ForMember(x => x.FeaturedImage, y => y.MapFrom(src => src.Galleries.FirstOrDefault(opt => opt.IsFeature).ImageUrl))
                .ForMember(x => x.Rating, y => y.MapFrom(src => src.Ratings.Count == 0 ? 5 : (double)src.Ratings.Sum(r => r.Ratings) / ((double)src.Ratings.Count)))
                .ForMember(x => x.NumberOfReviews, y => y.MapFrom(src => src.Reviews.Count));

            CreateMap<GalleryDto, Gallery>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelDto>().ReverseMap();
            CreateMap<Hotel, AddHotelResponseDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>()
                .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
                .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => src.Ratings.Count == 0 ? 0 : (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
                .ForMember(hotel => hotel.NumberOfReviews, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(hotel => hotel.Gallery, opt => opt.MapFrom(src => src.Galleries.Select(gallery => gallery.ImageUrl).ToList()));

            CreateMap<Hotel, GetAllHotelDto>()
               .ForMember(hotel => hotel.FeaturedImage, opt => opt.MapFrom(src => src.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl))
               .ForMember(hotel => hotel.Rating, opt => opt.MapFrom(src => src.Ratings.Count == 0 ? 0 : (double)src.Ratings.Sum(customer => customer.Ratings) / ((double)src.Ratings.Count)))
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
        
            CreateMap<Customer, GetUsersResponseDto>()
                .ForMember(x => x.FirstName, y => y.MapFrom(u => u.AppUser.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(u => u.AppUser.LastName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.Id, y => y.MapFrom(u => u.AppUser.Id))
                .ForMember(x => x.Email, y => y.MapFrom(u => u.AppUser.Email))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(u => u.AppUser.PhoneNumber))
                .ForMember(x => x.UserName, y => y.MapFrom(u => u.AppUser.UserName))
                .ForMember(x => x.Age, y => y.MapFrom(u => u.AppUser.Age))
                .ForMember(x => x.State, y => y.MapFrom(u => u.State))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(u => u.AppUser.CreatedAt));

            //Review Maps
            CreateMap<Review, ReviewToReturnDto>().ReverseMap();
            CreateMap<Review, AddReviewDto>().ReverseMap();

            //Manager Maps
            CreateMap<Manager, ManagerDto>().ReverseMap();

            CreateMap<AppUser, ManagerDto>()
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.Email))
                .ForMember(manager => manager.BusinessEmail, u => u.MapFrom(user => user.UserName))
                .ForMember(manager => manager.BusinessPhone, u => u.MapFrom(user => user.PhoneNumber))
                .ReverseMap();

            CreateMap<Manager, ManagerResponseDto>()
                .ForMember(d => d.FirstName, o => o.MapFrom(u => u.AppUser.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(u => u.AppUser.LastName))
                .ForMember(d => d.Gender, o => o.MapFrom(u => u.AppUser.Gender))
                .ForMember(d => d.Age, o => o.MapFrom(u => u.AppUser.Age))
                .ReverseMap();


            //AppUser Maps
            CreateMap<AppUser, ManagerResponseDto>().ReverseMap();
        }
    }
}
