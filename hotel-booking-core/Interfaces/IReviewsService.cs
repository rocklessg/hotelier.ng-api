using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_booking_dto.commons;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewsService
    {

        Task<Response<Paginator.PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging,
            string hotelId);
        Task<Response<bool>> AddReviewAsync(AddReviewDto review);
    }
}