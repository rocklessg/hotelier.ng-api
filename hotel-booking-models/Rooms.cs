namespace hotel_booking_models
{
    public class Rooms : BaseModel
    {
        public string RoomTypeId { get; set; }
        public string RoomNo { get; set; }
        public bool IsBooked { get; set; }
        public RoomTypes Roomtype { get; set; }
    }
}