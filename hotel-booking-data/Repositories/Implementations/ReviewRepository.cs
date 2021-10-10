using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public int TotalCount { get; set; }

        public async Task<bool> AddReviewAsync(Review review)
        {
            await _reviews.AddAsync(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsByHotelAsync(string HotelId, int page, int perPage)
        {
            var reviews = _reviews
                .Where(reviews => reviews.HotelId == HotelId)
                .OrderByDescending(reviews => reviews.CreatedAt);

            TotalCount = await reviews.CountAsync();
            var paginateReviews = await Paginator(reviews, page, perPage);

            return paginateReviews;

        }

        private static async Task<IEnumerable<Review>> Paginator(IQueryable<Review> reviews, int page, int perPage)
        {
            return await reviews.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }
    }
}
