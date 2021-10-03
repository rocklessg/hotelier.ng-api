using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class HotelStatisticDto
    {
        public string Name { get; set; }
        public string NumberOfRooms { get; set; }
        public string AverageRatings { get; set; }
        public string RoomsOccupied { get; set; }
        public string RoomsUnOccupied { get; set; }
        public string NumberOfReviews { get; set; }
        public string TotalNumberOfBookings { get; set; }
        public string TotalEarnings { get; set; }
        public string RoomTypes { get; set; }
        public string NumberOfAmenities { get; set; }

    }
}
