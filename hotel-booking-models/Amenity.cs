namespace hotel_booking_models
{
    public class Amenity : BaseModel
    {
        public string HotelId  { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } = 0.00;
        public double Discount { get; set; } = 0.00;
        public Hotel Hotel { get; set; }
    }
}