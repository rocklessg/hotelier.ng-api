using hotel_booking_dto;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_core.Interfaces;
using System.Threading.Tasks;
using System.Security.Claims;
using hotel_booking_dto.CustomerDtos;
using Serilog;
using hotel_booking_utilities;
using hotel_booking_dto.commons;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        public CustomerController(ICustomerService customerService, ILogger logger, UserManager<AppUser> userManager)
        {
            _customerService = customerService;
            _logger = logger;
            _userManager = userManager;
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Response<string>>> Update([FromBody] UpdateCustomerDto model)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            _logger.Information($"Update Attempt for user with id = {userId}");
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

            _logger.Information($"Update Image Attempt for user with id = {userId}");
            var result = await _customerService.UpdatePhoto(imageDto, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("AllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetAllCustomersAsync([FromQuery] PagingDto pagenator)
        {
            var result = await _customerService.GetAllCustomersAsync(pagenator);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("wishlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerWishList([FromQuery] PagingDto paging)
        {
            string customerId = _userManager.GetUserId(User);
            _logger.Information($"Retrieving the paginated wishlist for the customer with ID {customerId}");
            var result = await _customerService.GetCustomerWishList(customerId, paging);
            _logger.Information($"Retrieved the paginated wishlist for the customer with ID {customerId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{customerId}/whishlist")]
        public async Task<IActionResult> GetCustomerHotelsIdAsync(string customerId,[FromQuery]PagingDto paging)
        {
            var result = await _customerService.GetCustomerHotelsAsync(customerId, paging);
            return StatusCode(result.StatusCode, result);
        }
    }
}
