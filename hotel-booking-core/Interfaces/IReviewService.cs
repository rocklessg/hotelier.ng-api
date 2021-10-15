using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewService
    {

        Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging,
            string hotelId);

        Task<Response<ReviewToReturnDto>> AddReviewAsync(AddReviewDto model, string customerId);
        Response<string> UpdateUserReview(string customerId, ReviewRequestDto reviewRequestDto);
    }
}