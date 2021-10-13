using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_dto;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Http;
using hotel_booking_core.Interfaces;
using System.Threading.Tasks;
using System.Security.Claims;
using hotel_booking_dto.CustomerDtos;
using Microsoft.Extensions.Logging;
using hotel_booking_dto.HotelDtos;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IHotelService _hotelService;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService customerService, IHotelService hotelService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _hotelService = hotelService;
            _logger = logger;
        }


        [HttpPut("update" )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Response<string>>> Update([FromBody] UpdateCustomerDto model)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation($"Update Attempt for user with id = {userId}");
            var result = await _customerService.UpdateCustomer(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        
        [HttpPatch("update-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateImage([FromForm] AddImageDto imageDto)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation($"Update Image Attempt for user with id = {userId}");
            var result = await _customerService.UpdatePhoto(imageDto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{hotelId}/book-hotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateBooking([FromRoute] string hotelId, [FromBody] HotelBookingRequestDto bookingDto)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var result = await _hotelService.BookHotel(hotelId, userId, bookingDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
