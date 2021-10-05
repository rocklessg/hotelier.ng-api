using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.Mapper;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelService : IHotelService
    {
        private readonly ILogger<HotelService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelService(ILogger<HotelService> logger, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<HotelBasicDto>> GetHotelsByRatingsAsync(Paging paging)
        {
            var hotelList = await _unitOfWork.Hotels.GetAllAsync(
                orderby: x => x.OrderBy(h => h.Ratings.Sum(r => r.Ratings)),
                Includes: new List<string>() { "Galleries" }
                );
            var dtoList = _mapper.Map<List<HotelBasicDto>>(hotelList);
            dtoList.ForEach(dto => dto.thumbnails = dto.Galleries.FirstOrDefault(pic => pic.IsFeature).ImageUrl);
            return dtoList.Skip(paging.PageNumber - 1).Take(paging.PageSize).ToList();
        }

        public async Task GetTopDeals()
        {
            return;
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

        public async Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId)
        {
            var room = await _unitOfWork.Hotels.HotelRatings(hotelId);

            if (room.Count() > 0)
            {
                var response = _mapper.Map<IEnumerable<HotelRatingsDTo>>(room);

                var result = new Response<IEnumerable<HotelRatingsDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"cummulated ratings for hotel with id {hotelId}",
                    Data = response
                };

                return result;
            }
            return new Response<IEnumerable<HotelRatingsDTo>>();
        }

    }
}
