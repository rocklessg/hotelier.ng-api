using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
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

        //public AddAmenityDto AddAmenity(AddAmenityDto amenity)
        //{
            
        //}

        public Amenity GetAmenityById(string id)
        {
            var amenity = _context.Amenities.FirstOrDefault(x => x.Id == id);
            return amenity;
        }

        //public Amenity AddAmenityToHotel(string id)
        //{
        //    var hotel = _context.Hotels.FirstOrDefault(x => x.Id == id);

        //}


    }
}
