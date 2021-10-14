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
        public string Thumbnail { get; set; }
        public double PercentageRating { get; set; }
        public decimal Price { get; set; }
    }
}
