using AutoMapper;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_models;
using System.Linq;

namespace hotel_booking_utilities
{
    public class HotelBasicDtoMapper
    {
        public static HotelBasicDto MapToHotelBAsicDto(RoomType hotel,IMapper mapper)
        {
            var hotelDto = mapper.Map<HotelBasicDto>(hotel);
            hotelDto.Thumbnails = hotel.Galleries.FirstOrDefault(pic => pic.IsFeature).ImageUrl;
            return hotelDto;
        }

        public static RoomInfoDTo MapToRoomInfoDto(RoomType roomType, IMapper mapper)
        {
            var roomInfoDto = mapper.Map<RoomInfoDTo>(roomType);
            return 
        }
    }
}
