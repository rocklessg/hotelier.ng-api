using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Search;

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
        

        public async Task<bool> AddReviewAsync(Review review)
        {
            await _reviews.AddAsync(review);
            return await _context.SaveChangesAsync() > 0;
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
