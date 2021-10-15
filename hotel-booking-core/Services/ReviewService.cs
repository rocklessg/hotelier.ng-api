using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        

        public ReviewService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           
        }

        public Response<string> UpdateUserReview(string customerId, ReviewRequestDto reviewRequestDto)
        {
            var response = new Response<string>();
            var review = _unitOfWork.Reviews.GetUserReview(reviewRequestDto.reviewId);


            if (review != null)
            {
                if (review.CustomerId == customerId)
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

        public async Task<Response<ReviewToReturnDto>> AddReviewAsync(AddReviewDto model, string customerId)
        {
            var response = new Response<ReviewToReturnDto>();
           
            
            var checkHotel = await  _unitOfWork.Reviews.CheckReviewByCustomerAsync(model.HotelId);
            if (checkHotel == null)
            {
                response.Succeeded = false;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            var review =  _mapper.Map<Review>(model);
            review.CustomerId = customerId;
            await _unitOfWork.Reviews.InsertAsync(review);
            await _unitOfWork.Save();
           
            var reviewToReturn = _mapper.Map<ReviewToReturnDto>(review);
            
            //response
            response.Succeeded = true;
            response.Data = reviewToReturn;
            response.Message = "Review added successfully";
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }
 
        public async Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging, string hotelId)
        {
            var response =  new Response<PageResult<IEnumerable<ReviewToReturnDto>>>();
            var hotelExistCheck = await _unitOfWork.Hotels.GetHotelsById(hotelId);

            if (hotelExistCheck == null)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var hotel = _unitOfWork.Reviews.GetAllReviewsByHotelAsync(hotelId);
      
            var pageResult = await hotel.PaginationAsync<Review, ReviewToReturnDto>(paging.PageSize, paging.PageNumber, _mapper);

            response.Succeeded = true;
            response.Data = pageResult;
            response.Message = $"List of all reviews in hotel with id {hotelId}";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }
    }
}
