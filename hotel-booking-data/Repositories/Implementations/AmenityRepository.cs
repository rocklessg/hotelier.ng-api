using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly HbaDbContext _context;

        public AmenityRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public Amenity GetAmenityById(string id)
        {
            var amenity = _context.Amenities.FirstOrDefault(x => x.Id == id);
            return amenity;
        }



        public Hotel GetAmenityByHotelId(string hotelId)
        {

            var hotel = _context.Hotels.Include(x => x.Amenities).FirstOrDefault(x => x.Id == hotelId);
            return hotel;
        }
    }
}
