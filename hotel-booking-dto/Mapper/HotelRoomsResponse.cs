using hotel_booking_dto.commons;
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
        public static IEnumerable<RoomsByHotelDTo> GetResponse(List<Room> room)
        {
            var newList = new List<RoomsByHotelDTo>();

            for (int i = 0; i < room.Count(); i++)
            {
                var listNew = new RoomsByHotelDTo
                {
                    Id = room[i].Id,
                    IsBooked = room[i].IsBooked,
                    RoomtypeName = room[i].Roomtype.Name,
                    RoomTypeThumbnail = room[i].Roomtype.Thumbnail
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
