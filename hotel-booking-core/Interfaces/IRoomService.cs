using hotel_booking_dto;
using hotel_booking_dto.RoomsDtos;

namespace hotel_booking_core.Interfaces
{
    public interface IRoomService
    {
        Response<RoomDTo> GetHotelRooomById(string roomId);
    }
}