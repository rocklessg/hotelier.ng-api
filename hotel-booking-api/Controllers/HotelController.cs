using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelService _hotelService;

        public HotelController(ILogger<HotelController> logger, IHotelService hotelService)
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        [HttpGet]
        [Route("top-hotels")]
        public async Task<ActionResult<Response<List<HotelBasicDto>>>> HotelsByRatingsAsync([FromQuery] hotel_booking_utilities.Paginator paginator)
        {
            var response = await _hotelService.GetHotelsByRatingsAsync(paginator);
            //var response = Response<List<HotelBasicDto>>.Success(result);
            //response.StatusCode = StatusCodes.Status200OK;
            return StatusCode(response.StatusCode, response);
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
