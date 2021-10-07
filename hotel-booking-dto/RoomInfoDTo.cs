using hotel_booking_models;

namespace hotel_booking_dto
{
    public class RoomInfoDTo
    {
        public string HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Thumbnail { get; set; }
        public RoomType Hotel { get; set; }
    }
}
