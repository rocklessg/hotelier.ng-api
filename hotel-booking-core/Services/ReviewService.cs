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
            var response = new Response<string>();

            var review =  _unitOfWork.Reviews.GetUserReview(reviewRequestDto.HotelId)
                                             .FirstOrDefault(x => x.CustomerId == customerId);

            if(review != null)
            {
                review.Comment = reviewRequestDto.Comment;
                _unitOfWork.Reviews.Update(review);
                _unitOfWork.Save();

                response.Data = default;
                response.Succeeded = true;
                response.StatusCode = (int)HttpStatusCode.Created;
                response.Message = $"Comment updated successfully";

                return response;
            }

            response.Data = default;
            response.Succeeded = false;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"No review exist";

            return response;
        }
    }
}
