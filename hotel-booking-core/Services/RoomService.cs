using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.Mapper;
using hotel_booking_dto.RoomsDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            return Response<RoomDTo>.Fail("Not Found");
        }

        public async Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelid, AddRoomDto roomDto)
        {
            Room room = _mapper.Map<Room>(roomDto);

            string message = "room data or hotel id is empty";

            room.Id = Guid.NewGuid().ToString();
            room.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Rooms.InsertAsync(room);
            await _unitOfWork.Save();

            var roomResponse = _mapper.Map<AddRoomResponseDto>(room);

            var response = new Response<AddRoomResponseDto>()
            {
                StatusCode = room.Id != null ? 200 : 400,
                Succeeded = room.Id != null ? true : false,
                Data = roomResponse,
                Message = room.Id != null ? $"Room with id {room.Id} added to Hotel with id {hotelid}" : message
            };

            return response;
        }
    }
}
