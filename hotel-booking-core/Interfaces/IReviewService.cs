using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IReviewService
    {
        Response<string> UpdateUserReview(string customerId, ReviewRequestDto reviewRequestDto);
    }
}
