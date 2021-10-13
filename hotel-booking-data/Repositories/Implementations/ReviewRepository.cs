using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {

        private readonly HbaDbContext _context;
        private readonly DbSet<Review> _reviews;

        public ReviewRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _reviews = _context.Set<Review>();
        }
        

        public async Task<Review> CheckReviewByCustomerAsync(string customerId, string hotelId)
        {
            return await _reviews.Where(x => x.CustomerId == customerId && x.HotelId == hotelId).SingleOrDefaultAsync();
        }

        public IQueryable<Review> GetAllReviewsByHotelAsync(string hotelId)
        {
            var query = _reviews.AsNoTracking();
            query = query.Include(h => h.Hotel);
            query = query.Where(r => r.HotelId == hotelId);
            query = query.OrderBy(r => r.CreatedAt);
            return query;
        }
    }
}
