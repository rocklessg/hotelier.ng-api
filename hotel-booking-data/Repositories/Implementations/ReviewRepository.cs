using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly HbaDbContext _context;
        public ReviewRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetUserReview(string hotelId)
        {
            var hotelReviews =  _context.Reviews.Where(x => x.HotelId == hotelId);
            
            return hotelReviews;
        }
    }
}
