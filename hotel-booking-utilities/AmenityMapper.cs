using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public static class AmenityMapper
    {
        public static AmenityDto MapToAmenityDTO(Amenity amenity)
        {
            return new AmenityDto
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Price = amenity.Price,
                Discount = amenity.Discount
            }; 
            
        }            
    }
}
