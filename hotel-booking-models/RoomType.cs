using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class RoomType : BaseModel
    {
        public string HotelId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
        public double Price { get; set; } = 0.00;
        public double Discount { get; set; } = 0.00;
        public Hotel Hotel { get; set; }
        public ICollection<Room> Room { get; set; }
    }
}