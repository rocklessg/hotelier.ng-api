using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_utilities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        /// <summary>
        /// Fetches all hotels in database. Returns a List of all registered hotels.
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<Response<List<GetHotelDto>>> GetAllHotelsAsync(PagingDto paging);
        /// <summary>
        /// Fetches and hotel using it's Id. Returns the hotel object and it's child entities
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Response<GetHotelDto> GetHotelById(string id);
        /// <summary>
        /// Updates an hotel asynchronously and returns update hotel response
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model);
        Task<Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>> GetHotelRoomType(PagingDto paging, string hotelId);
        Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId);
        Task<Response<IEnumerable<RoomDTo>>> GetHotelRooomById(string hotelId, string roomTypeId);
        Task<Response<AddHotelResponseDto>> AddHotel(string managerId, AddHotelDto hotelDto);
        Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto);
        Task<Response<string>> DeleteHotelByIdAsync(string hotelId);
        Task<Response<IEnumerable<HotelAndroidDto>>> GetHotelsByRatingsAsync();
        Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto);
        Task<Response<IEnumerable<HotelAndroidDto>>> GetTopDealsAsync();

        /// <summary>
        /// Searches for hotels that are within the provided location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="paginator"></param>
        /// <returns>Returns an IEnumerable of hotels within the input location. Returns an empty array is search doesn't match any location in records</returns>
        Task<Response<PageResult<IEnumerable<HotelAndroidDto>>>> GetHotelByLocation(string location, PagingDto paging);
        Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging, string hotelId);
    }
}
