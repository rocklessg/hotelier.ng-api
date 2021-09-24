namespace hotel_booking_models
{
    public class Amenities : BaseModel
    {
        public string HotelId  { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } = 0.00;
        public double Discount { get; set; } = 0.00;
        public Hotels Hotel { get; set; }
    }
}