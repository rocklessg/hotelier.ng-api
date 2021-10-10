using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;

namespace hotel_booking_data.Repositories.Implementations
{
    public class WishListRepository : GenericRepository<WishList>, IWishListRepository
    {
        private readonly HbaDbContext _context;

        public WishListRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
