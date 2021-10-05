using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly HbaDbContext _context;

        public AmenityRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public UpdateAmenityDto UpdateAmenity(string id, UpdateAmenityDto update)
        {
            
            var updatedAmenity = _context.Amenities.Update(update);
        }
    }
}
