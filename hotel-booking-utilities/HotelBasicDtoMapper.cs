using AutoMapper;
using hotel_booking_dto.commons;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public class HotelBasicDtoMapper
    {
        public static HotelBasicDto MapToHotelBAsicDto(Hotel hotel,IMapper mapper)
        {
            var hotelDto = mapper.Map<HotelBasicDto>(hotel);
            hotelDto.Thumbnails = hotel.Galleries.FirstOrDefault(pic => pic.IsFeature).ImageUrl;
            return hotelDto;
        }
    }
}
