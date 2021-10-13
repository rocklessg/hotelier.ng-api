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
using hotel_booking_utilities;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService customerService, IBookingService bookingService, ILogger<CustomerController> logger)
        {
            HttpClientInitializer.Initialize();
            _customerService = customerService;
            _bookingService = bookingService;
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
            var result = await _bookingService.Book(hotelId, userId, bookingDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{userId}/bookings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerBooking([FromRoute] string userId, [FromQuery] Paginator paginator)
        {
            var result = await _bookingService.GetCustomerBookings(userId, paginator);
            return StatusCode(result.StatusCode, result);
        }
    }
}
