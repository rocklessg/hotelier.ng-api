using hotel_booking_dto.commons;

namespace hotel_booking_dto
{
    public class RoombyPriceDto : Paginator
    {
        public bool IsBooked { get; set; }
        public decimal Price { get; set; }
    }
}
