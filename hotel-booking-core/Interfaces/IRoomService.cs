using hotel_booking_dto;
using hotel_booking_dto.RoomsDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IRoomService
    {
        Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto);
    }
}