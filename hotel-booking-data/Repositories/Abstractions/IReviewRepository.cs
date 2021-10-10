using hotel_booking_models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        int TotalCount { get; set; }
        Task<IEnumerable<Review>> GetAllReviewsByHotelAsync(string livestockId, int page, int perPage);
        Task<bool> AddReviewAsync(Review review);
    }
}
