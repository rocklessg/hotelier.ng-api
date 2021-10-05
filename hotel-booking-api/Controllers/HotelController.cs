using hotel_booking_data.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HotelController(ILogger<HotelController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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
    }
}
