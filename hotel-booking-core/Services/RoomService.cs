using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.Mapper;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<IEnumerable<RoomsByHotelDTo>>> GetAvailableRoomByHotel(Paginator paginator, string hotelId)
        {
            var roomList = await _unitOfWork.Rooms.GetAvailableRoomsByHotel(hotelId);

            if (roomList.Count() > 0)
            {
                var dtoList = HotelRoomsResponse.GetResponse(roomList);

                var item = dtoList.Skip(paginator.PageSize * (paginator.CurrentPage - 1))
                .Take(paginator.PageSize);

                var result = new Response<IEnumerable<RoomsByHotelDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = "available rooms",
                    Data = item
                };

                return result;
            }
            return new Response<IEnumerable<RoomsByHotelDTo>>();
        }

        public Response<RoomDTo> GetHotelRooomById(string roomId)
        {
            var room = _unitOfWork.Rooms.GetHotelRoom(roomId);

            if (room != null)
            {
                var response = HotelRoomsResponse.GetResponse(room);

                var result = new Response<RoomDTo>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"is the room with id {roomId}",
                    Data = response
                };
                return result;
            }
            return new Response<RoomDTo>();
        }
    }
}
