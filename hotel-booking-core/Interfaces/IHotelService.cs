using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        Task<IList<HotelBasicDto>> GetHotelsByRatingsAsync(Paging paging);
        
        Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId);
    }
}
