namespace hotel_booking_models
{
    public class WishLists
    {
        public string CustomerId { get; set; }
        public string HotelId { get; set; }
        public Customer Customer { get; set; }
        public Hotels Hotel { get; set; }
    }
}