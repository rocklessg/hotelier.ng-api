using hotel_booking_dto;
using hotel_booking_dto.RoomsDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IRoomService
    {
        Response<RoomDTo> GetHotelRooomById(string roomId);
        Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto);
    }
}