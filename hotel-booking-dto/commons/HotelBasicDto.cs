using hotel_booking_models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hotel_booking_dto.commons
{
    public class HotelBasicDto
    {
        [Display(Name = "ManagerId")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Thumbnails { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
    }

    public class RoomsByHotelDTo
    {
        public string Id { get; set; }
        public bool IsBooked { get; set; }
        public string RoomtypeName { get; set; }
        public string RoomTypeThumbnail { get; set; }

    }

    public class RoomDTo
    {
        public string Id { get; set; }
        public string RoomNo { get; set; }
        public bool IsBooked { get; set; }
        public string RoomType { get; set; }

    }

    public class HotelRatingsDTo
    {
        public string Id { get; set; }
        public int Ratings { get; set; }
        public string CustomerId { get; set; }

    }
}
