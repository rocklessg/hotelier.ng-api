using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
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

        //[Authorize("Manager")]
        [HttpPut("{hotelId}")]
        public async Task<IActionResult> UpdateHotel(string hotelId, [FromBody] UpdateHotelDto update)
        {
            var response = await _hotelService.UpdateHotelAsync(hotelId, update);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet]
        [Route("top-hotels")]
        public async Task<ActionResult<Response<List<HotelBasicDto>>>> HotelsByRatingsAsync([FromQuery] Paging paging)
        {
            var response = await _hotelService.GetHotelsByRatingsAsync(paging);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("top-deals")]
        public IActionResult TopDealsAsync()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetHotelRoomsByPrice(RoombyPriceDto pricing)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}/room")]
        public async Task<IActionResult> GetAvailableHotelAsync([FromQuery] Paginator paginator, string id)
        {
            var rooms = await _hotelService.GetAvailableRoomByHotel(paginator, id);
            return Ok(rooms);
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
