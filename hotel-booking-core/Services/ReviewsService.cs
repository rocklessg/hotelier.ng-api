using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelService _hotelService;
       
        private readonly IMapper _mapper;
        

        public ReviewsService(IUnitOfWork unitOfWork, IHotelService hotelService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
            _mapper = mapper;
           
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
