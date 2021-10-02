using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly HbaDbContext _context;

        public RoomRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
