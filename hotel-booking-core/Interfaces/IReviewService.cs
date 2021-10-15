using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewService
    {

        Task<Response<ReviewToReturnDto>> AddReviewAsync(AddReviewDto model, string customerId);
        Response<string> UpdateUserReview(string customerId, string reviewId, ReviewRequestDto model);
        
    }
}