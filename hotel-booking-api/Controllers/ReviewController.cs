using hotel_booking_core.Interfaces;
using hotel_booking_dto.ReviewDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPatch("hotelId")]
        public IActionResult UpdateCustomerReview([FromBody]ReviewRequestDto reviewRequestDto)
        {
            // var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var customerId = "d296434c-13bc-4f44-a8d7-4426bf0cbd56";
            var response = _reviewService.UpdateUserReview(customerId, reviewRequestDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
