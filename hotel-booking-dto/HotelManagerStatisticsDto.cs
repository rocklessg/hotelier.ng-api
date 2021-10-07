using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class HotelManagerStatisticsDto
    {
        public int NumberOfHotels { get; set; }
        public double AverageHotelRatings { get; set; }
        public int TotalNumberOfCustomers { get; set; }
        public int TotalManagerRooms { get; set; }
        public int TotalManagerUnoccupiedRooms { get; set; }
        public int TotalManagerOccupiedRooms { get; set; }
        public decimal TotalManagerTransactionAmount { get; set; }

    }
}
