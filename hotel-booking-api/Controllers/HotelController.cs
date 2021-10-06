using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelService _hotelService;

        public HotelController(ILogger<HotelController> logger, IUnitOfWork unitOfWork,
            IHotelService hotelService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
        }

        [HttpGet]
        [Route("top-hotels")]
        public IActionResult HotelsByRatings()
        {
            return Ok();
        }

        [HttpGet]
        [Route("top-deals")]
        public IActionResult TopDeals()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetHotelRooms()
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}/rooms")]
        public async Task<IActionResult> GetAvailableHotelAsync([FromQuery] Paginator paginator, string id)
        {
            var rooms = await _hotelService.GetAvailableRoomByHotel(paginator, id);
            return Ok(rooms);
        }

        [HttpGet]
        [Route("room")]
        public IActionResult HotelRoomById(string id)
        {
            var room = _hotelService.GetHotelRooomById(id);
            return Ok(room);
        }

        [HttpGet]
        [Route("ratings/{id}")]
        public async Task<IActionResult> HotelRatingsAsync(string id)
        {
            var rating = await _hotelService.GetHotelRatings(id);
            return Ok(rating);
        }
    }
}
