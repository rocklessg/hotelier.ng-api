using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_models
{
    public class Hotel : BaseEntity
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
        public ICollection<WishList> WishList { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Rating> Rating { get; set; }
        public ICollection<RoomType> RoomType { get; set; }
        public ICollection<Amenity> Amenity { get; set; }
    }
}