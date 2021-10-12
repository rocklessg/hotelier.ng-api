using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;

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
                    Price = roomType[i].Price,
                    Thumbnail = roomType[i].Thumbnail
                };

                newList.Add(listNew);
            }
            return newList;
        }
        public static List<RoomDTo> GetResponse(List<Room> room)
        {

            var newList = new List<RoomDTo>();

            for (int i = 0; i < room.Count; i++)
            {
                var listNew = new RoomDTo
                {
                    Id = room[i].Id,
                    RoomNo = room[i].RoomNo,
                    IsBooked = room[i].IsBooked
                };

                newList.Add(listNew);
            }
            return newList;

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
