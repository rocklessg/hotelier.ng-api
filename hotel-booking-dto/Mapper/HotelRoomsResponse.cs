using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RoomsDtos;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.Mapper
{
    public class HotelRoomsResponse
    {
        public static IEnumerable<RoomTypeByHotelDTo> GetResponse(List<RoomType> roomType) 
        {
            var newList = new List<RoomTypeByHotelDTo>();

            for (int i = 0; i < roomType.Count(); i++)
            {
                var listNew = new RoomTypeByHotelDTo
                {
                    Id = roomType[i].Id,
                    Name = roomType[i].Name,
                    TotalBookRoom = roomType[i].Rooms.Where(x => x.IsBooked).Count(),
                    TotalUnbookedRoom = roomType[i].Rooms.Where(x => !x.IsBooked).Count(),
                    Thumbnail = roomType[i].Thumbnail
                };

                newList.Add(listNew);
            }
            return newList;
        }
        public static RoomDTo GetResponse(Room room)
        {
            return new RoomDTo
            {
                Id = room.Id,
                RoomNo = room.RoomNo,
                IsBooked = room.IsBooked,
                RoomType = room.Roomtype.Name
            };
        }
        public static IEnumerable<HotelRatingsDTo> GetResponse(List<Rating> rating)
        {
            var newList = new List<HotelRatingsDTo>();

            for (int i = 0; i < rating.Count(); i++)
            {
                var listNew = new HotelRatingsDTo
                {
                    Id = rating[i].Id,
                    Ratings = rating[i].Ratings,
                    CustomerId = rating[i].CustomerId,
                };

                newList.Add(listNew);
            }
            return newList;
        }
    }
}
