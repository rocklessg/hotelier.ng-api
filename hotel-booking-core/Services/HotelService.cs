using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto.commons;
using hotel_booking_models;
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


    }
}
