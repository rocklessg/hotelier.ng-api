using System;

namespace hotel_booking_dto.BookingDtos
{
    public class CreateBookingResponseDto
    {
        public string BookReference { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumberOfPeople { get; set; }
        public string ServiceName { get; set; }
    }
}
