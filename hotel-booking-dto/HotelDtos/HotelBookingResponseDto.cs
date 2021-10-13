using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.HotelDtos
{
    public class HotelBookingResponseDto
    {
        public string BookingReference { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NoOfPeople { get; set; }
        public bool PaymentStatus { get; set; }
        public Room Room { get; set; }
        public string PaymentUrl {  get; set; }
    }
}
