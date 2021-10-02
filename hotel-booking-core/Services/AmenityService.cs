using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly HbaDbContext _context;

        public AmenityService(HbaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Amenity> GetAllAmenities()
        {
            var result = _context.Amenities;
            return result;
        }

        public IEnumerable<Amenity> GetAmenityByHotelId(string hotelId)
        {
            var collection = _context.Amenities;
            var selectedAmenities = collection.Where(amenity => amenity.HotelId == hotelId);
            return selectedAmenities;
        }
    }
}
