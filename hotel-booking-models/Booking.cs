using System;

namespace hotel_booking_models
{
    public class Booking : BaseEntity
    {
        public string CustomerId { get; set; }
        public string BookingReference { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NoOfPeople { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
    }
}