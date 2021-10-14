using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        /// <summary>
        /// Fetches all hotels in database. Returns a List of all registered hotels.
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<Response<List<GetHotelDto>>> GetAllHotelsAsync(Paginator paging);
        /// <summary>
        /// Fetches and hotel using it's Id. Returns the hotel object and it's child entities
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<GetHotelDto>> GetHotelByIdAsync(string id);
        /// <summary>
        /// Updates an hotel asynchronously and returns update hotel response
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model);
        Task<Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>> GetHotelRoomType(Paging paging, string hotelId);
        Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId);
        Task<Response<IEnumerable<RoomDTo>>> GetHotelRooomById(string hotelId, string roomTypeId);
        Task<Response<AddHotelResponseDto>> AddHotel(string managerId, AddHotelDto hotelDto);
        Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto);
        Task<Response<IEnumerable<HotelBasicDto>>> GetHotelsByRatingsAsync();
        Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto);
        Task<Response<IEnumerable<RoomInfoDto>>> GetTopDealsAsync();
    }
}
