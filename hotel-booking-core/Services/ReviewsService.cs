using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
       
        public async Task<Response<ReviewToReturnDto>> AddReviewAsync(AddReviewDto model)
        {
            var response = new Response<ReviewToReturnDto>();
           
            var currentUser = await  _unitOfWork.Reviews.CheckReviewByCustomerAsync(model.CustomerId, model.HotelId);
            if (currentUser == null)
            {
                response.Succeeded = false;
                response.Message = "Invalid Request";
                response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            var review =  _mapper.Map<Review>(model); 
            await _unitOfWork.Reviews.InsertAsync(review);
            await _unitOfWork.Save();

            var reviewToReturn = _mapper.Map<ReviewToReturnDto>(review);
            reviewToReturn.Id = model.CustomerId;
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
            var hotelExistCheck = _hotelService.GetHotelById(hotelId);

            if (!hotelExistCheck.Succeeded)
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var hotel = _unitOfWork.Reviews.GetAllReviewsByHotelAsync(hotelId);
            if (!(hotel.Any()))
            {
                response.Succeeded = false;
                response.Data = null;
                response.Message = "No reviews made for this hotel yet!";
                response.StatusCode = (int)HttpStatusCode.NoContent;
                return response;
            }
            var pageResult = await hotel.PaginationAsync<Review, ReviewToReturnDto>(paging.PageSize, paging.PageNumber, _mapper);

            response.Succeeded = true;
            response.Data = pageResult;
            response.Message = $"List of all reviews in hotel with id {hotelId}";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }
    }
}
