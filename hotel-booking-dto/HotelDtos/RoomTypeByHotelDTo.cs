using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class RoomTypeByHotelDTo
    {
        public string Id { get; set; }
        public bool IsBooked { get; set; }
        public string RoomtypeName { get; set; }
        public string RoomTypeThumbnail { get; set; }

    }
}
