using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto
{
    public class TransactionResponseDto
    {
        public string BookingId { get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NoOfPeople { get; set; }
        public string ServiceName { get; set; }
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string CustomerId { get; set; }
        public string CustomeName { get; set; }
        public string PaymentId { get; set; }
        public Decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
