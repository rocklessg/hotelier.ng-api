using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly HbaDbContext _context;

        public AmenityRepository(HbaDbContext context) : base(context)
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
