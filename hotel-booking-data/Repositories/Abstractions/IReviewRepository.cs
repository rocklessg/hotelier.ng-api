using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Review GetUserReview(string hotelId);
        IQueryable<Review> GetAllReviewsByHotelAsync(string HotelId);
        Task<Review> CheckReviewByCustomerAsync(string HotelId);
    }
}
