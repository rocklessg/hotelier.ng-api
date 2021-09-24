using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Hotels : BaseModel
    {
        public string ManagerId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Manager Manager { get; set; }
        public ICollection<WishLists> WishList { get; set; }
        public ICollection<Reviews> Review { get; set; }
        public ICollection<Ratings> Rating { get; set; }
        public ICollection<RoomTypes> RoomType { get; set; }
        public ICollection<Amenities> Amenity { get; set; }
    }
}