using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        Task<IList<HotelBasicDto>> GetHotelsByRatingsAsync(Paging paging);

        Task<Response<IEnumerable<RoomsByHotelDTo>>> GetAvailableRoomByHotel(Paginator paginator, string hotelId);

        Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId);
    }
}
