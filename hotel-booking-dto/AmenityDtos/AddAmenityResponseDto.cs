using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.AmenityDtos
{
    public class AddAmenityResponseDto
    {
        public string HotelId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public static Amenity Add(string id, AddAmenityRequestDto model)
        {
            return new Amenity
            {
                Id = Guid.NewGuid().ToString(),
                HotelId = id,
                Name = model.Name,
                Price = model.Price,
                Discount = model.Discount,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

    }

    
}
