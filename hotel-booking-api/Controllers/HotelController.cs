using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
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

        public HotelController(ILogger<HotelController> logger, IUnitOfWork unitOfWork, IHotelService hotelService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
        }

        [AllowAnonymous]
        [HttpGet("AllHotels")]
        public async Task<IActionResult> GetAllHotels([FromQuery]Paginator paging)
        {
            var response = await _hotelService.GetAllHotelsAsync(paging);
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("GetHotel/{id}")]
        public IActionResult GetHotelById(string id)
        {
            var response = _hotelService.GetHotelById(id);
            return StatusCode(response.StatusCode, response);
        }

       // [Authorize("Manager")]
        [HttpPut("Update Hotel")]
        public async Task<IActionResult> UpdateHotel([FromBody] UpdateHotelDto update)
        {
            var response = await _hotelService.UpdateHotelAsync(update);
            return StatusCode(response.StatusCode, response);
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
