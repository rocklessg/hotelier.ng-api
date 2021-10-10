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
        public string Name { get; set; }
        public int TotalBookRoom { get; set; }
        public int TotalUnbookedRoom { get; set; }
        public string Thumbnail { get; set; }

    }
}
