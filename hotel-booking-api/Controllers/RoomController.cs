using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto.HotelDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/Hotel")]
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


        [HttpPost]
        [Route("{hotelid}/rooms")]
        //[Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddHotelRoom([FromQuery] string hotelid, [FromBody] AddRoomDto roomDto)
        {
            var result = await _roomService.AddHotelRoom(hotelid, roomDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
