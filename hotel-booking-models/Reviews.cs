namespace hotel_booking_models
{
    public class Reviews : BaseModel
    {
        public string Review { get; set; }
        public Customer Customer { get; set; }
    }
}