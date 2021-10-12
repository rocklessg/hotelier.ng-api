using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        IQueryable<Review> GetAllReviewsByHotelAsync(string HotelId);
        Task<bool> AddReviewAsync(Review review);
    }
}
