namespace hotel_booking_models
{
    public class Ratings : BaseModel
    {
        public int Rating { get; set; }
        public string HotelId { get; set; }
        public string CustomerId { get; set; }
        public Hotels Hotel { get; set; }
        public Customer Customer { get; set; }
    }
}