using AutoMapper;
using hotel_booking_dto.commons;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;

namespace hotel_booking_utilities
{
    public class HotelBasicDtoMapper
    {
        public static HotelBasicDto MapToHotelBAsicDto(Hotel hotel, IMapper mapper)
        {
            var hotelDto = mapper.Map<HotelBasicDto>(hotel);
            hotelDto.Thumbnails = hotel.Galleries.FirstOrDefault(pic => pic.IsFeature).ImageUrl;
            return hotelDto;
        }

        public static List<HotelBasicDto> MapToHotelBAsicDtoList(List<Hotel> hotelList, IMapper mapper)
        {
            var dtoList = new List<HotelBasicDto>();
            if (hotelList.Count == 0)
            {
                return dtoList;
            }
            foreach (var hotel in hotelList)
            {
                var dto = MapToHotelBAsicDto(hotel, mapper);
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public static RoomInfoDto MapToRoomInfoDto(RoomType roomType, IMapper mapper)
        {
            var roomInfoDto = mapper.Map<RoomInfoDto>(roomType);
            roomInfoDto.HotelName = roomType.Hotel.Name;
            return roomInfoDto;
        }
        public static List<RoomInfoDto> MapToRoomInfoDtoList(List<RoomType> roomTypeList, IMapper mapper)
        {
            
            var dtoList = new List<RoomInfoDto>();
            if (roomTypeList.Count == 0)
            {
                return dtoList;
            }
            foreach (var roomType in roomTypeList)
            {
                var dto = MapToRoomInfoDto(roomType, mapper);
                dtoList.Add(dto);
            }
            return dtoList;
        }
    }
}
