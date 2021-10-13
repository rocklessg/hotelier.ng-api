using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;


namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly ILogger _logger;


        public HotelController(ILogger logger, IHotelService hotelService, UserManager<AppUser> userManager, IHotelStatisticsService hotelStatisticsService)
        {
            _hotelService = hotelService;
            _userManager = userManager;
            _hotelStatisticsService = hotelStatisticsService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("all-hotels")]
        public async Task<IActionResult> GetAllHotels([FromQuery] Paginator paging)
        {
            var response = await _hotelService.GetAllHotelsAsync(paging);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{hotelId}")]
        public IActionResult GetHotelById(string hotelId)
        {
            var response = _hotelService.GetHotelById(hotelId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{hotelId}")]
        public async Task<IActionResult> UpdateHotel(string hotelId, [FromBody] UpdateHotelDto update)
        {
            var response = await _hotelService.UpdateHotelAsync(hotelId, update);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet]
        [Route("top-hotels")]
        public async Task<IActionResult> HotelsByRatingsAsync()
        {
            var response = await _hotelService.GetHotelsByRatingsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("top-deals")]
        public async Task<IActionResult> TopDealsAsync()
        {
            var response = await _hotelService.GetTopDealsAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("search/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelByLocation(string location, [FromQuery] Paging paging)
        {
            var result = await _hotelService.GetHotelByLocation(location, paging);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("room-by-price")]
        public async Task<IActionResult> GetHotelRoomsByPriceAsync([FromQuery]PriceDto pricing)
        {
            var response = await _hotelService.GetRoomByPriceAsync(pricing);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("{hotelId}/roomTypes")]
        public async Task<IActionResult> GetHotelRoomTypeAsync([FromQuery] Paging paging, string hotelId)
        {
            var rooms = await _hotelService.GetHotelRoomType(paging, hotelId);
            return Ok(rooms);
        }

        [HttpGet]
        [Route("{hotelId}/room/{roomTypeId}")]
        public async Task<IActionResult> HotelRoomById(string hotelId, string roomTypeId)
        {
            var room = await _hotelService.GetHotelRooomById(hotelId, roomTypeId);
            return Ok(room);
        }

        [HttpGet]
        [Route("{hotelId}/ratings")]
        public async Task<IActionResult> HotelRatingsAsync(string hotelId)
        {
            var rating = await _hotelService.GetHotelRatings(hotelId);
            return Ok(rating);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddHotel([FromBody] AddHotelDto hotelDto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var result = await _hotelService.AddHotel(loggedInUser.Id, hotelDto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{hotelId}/statistics")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotelStatistics(string hotelId)
        {
            _logger.Information($"About Getting statistics for hotel with ID {hotelId}");
            var result = await _hotelStatisticsService.GetHotelStatistics(hotelId);
            _logger.Information($"Gotten stats for hotel with ID {hotelId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("rooms/{hotelId}")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddHotelRoom(string hotelId, [FromBody] AddRoomDto roomDto)
        {
            var result = await _hotelService.AddHotelRoom(hotelId, roomDto);
            return StatusCode(result.StatusCode, result);

        }
    }
}
