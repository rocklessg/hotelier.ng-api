using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using System.Linq;
using System.Net;

namespace hotel_booking_core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<string> UpdateUserReview(string customerId, ReviewRequestDto reviewRequestDto)
        {
            Response<string> response = new Response<string>();
            var review = _unitOfWork.Reviews.GetUserReview(reviewRequestDto.reviewId);
                                           

            if(review != null)
            {
                if(review.CustomerId == customerId)
                {
                    review.Comment = reviewRequestDto.Comment;
                    _unitOfWork.Reviews.Update(review);
                    _unitOfWork.Save();

                    response.Succeeded = true;
                    response.StatusCode = (int)HttpStatusCode.Created;
                    response.Message = $"Comment updated successfully";

                    return response;
                }
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Message = $"You are not authorized to access this resource";

                return response;
            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Review with the Id {reviewRequestDto.reviewId} does not exist";

            return response;
        }
    }
}
