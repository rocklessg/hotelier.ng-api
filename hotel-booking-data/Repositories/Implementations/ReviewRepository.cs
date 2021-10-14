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

        public Review GetUserReview(string reviewId)
        {
            var custonerReview =  _context.Reviews.SingleOrDefault(x => x.Id == reviewId);
            
            return custonerReview;
        }
    }
}
