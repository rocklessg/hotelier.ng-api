using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        private readonly int _pageSize;
        private readonly int _pageNumber;
        

        public ReviewsService(IUnitOfWork unitOfWork, IHotelService hotelService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
            _mapper = mapper;
        }
        public int TotalCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<Response<bool>> AddReviewAsync(AddReviewDto review)
        {
            throw new NotImplementedException();
        }
 
        public async Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(string hotelId)
        {
            var hotel = _unitOfWork.Reviews.GetAllReviewsByHotelAsync(hotelId);
           

            
            var pageResult = await hotel.PaginationAsync<Review, ReviewToReturnDto>(_pageSize, _pageNumber, _mapper);

            var response =
                new Response<PageResult<IEnumerable<ReviewToReturnDto>>>(StatusCodes.Status200OK, true, "All reviews",
                    pageResult);
            return response;
        }
    }
}
