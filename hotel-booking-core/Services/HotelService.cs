using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
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

        public HotelService(ILogger<HotelService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<List<HotelBasicDto>>> GetHotelsByRatingsAsync(hotel_booking_utilities.Paginator paginator)
        {
            var hotelList = await _unitOfWork.Hotels.GetAllAsync(
                orderby: x => x.OrderBy(h => h.Ratings.Sum(r => r.Ratings) / h.Ratings.Count),
                Includes: new List<string>() { "Galleries"}
                );
            hotelList = hotelList.Skip(paginator.CurrentPage - 1).Take(paginator.PageSize).ToList();
            var hoteldtoList = new List<HotelBasicDto>();
            foreach (var hotel in hotelList)
            {
                hoteldtoList.Add(HotelBasicDtoMapper.MapToHotelBAsicDto(hotel, _mapper));
            }
            var response = new Response<List<HotelBasicDto>>();
            response.Data = hoteldtoList;
            response.Message = "List of Top Hotels by their ratings";
            response.Succeeded = true;
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }

        public async Task GetRoomByPrice(RoombyPriceDto priceDto)
        {
            var roomList = await _unitOfWork.Rooms.GetAllAsync(
                Includes: new List<string>() { "Roomtype" },
                expression: (room => room.IsBooked == priceDto.IsBooked && room.Roomtype.Price >= priceDto.Price),
                orderby: x => x.OrderBy(x => x.Roomtype.Price)
                );
        }

        public async Task GetTopDeals()
        {
            var roomList = await _unitOfWork.Rooms.GetAllAsync(
                Includes: new List<string>() { "Roomtype" },
                orderby: x => x.OrderBy(x => x.Roomtype.Discount)
                );            
        }
    }
}
