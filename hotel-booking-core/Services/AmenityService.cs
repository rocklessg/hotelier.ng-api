using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<Amenity> GetAmenityByHotelId(string hotelId)
        {
            var hotel = _context.Hotels.Include("Amenities").FirstOrDefault(x => x.Id == hotelId);
            var selectedAmenities = hotel.Amenities.ToList();
            return selectedAmenities;
        }
    }
}
