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
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IUnitOfWork unitOfWork,
            IRoomService roomService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult HotelRoomById(string id)
        {
            var room = _roomService.GetHotelRooomById(id);
            return Ok(room);
        }
    }
}
