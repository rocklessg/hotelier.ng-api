using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class RoomTypes : BaseModel
    {
        public string HotelId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
        public double Price { get; set; } = 0.00;
        public double Discount { get; set; } = 0.00;
        public Hotels Hotel { get; set; }
        public ICollection<Rooms> Room { get; set; }
    }
}