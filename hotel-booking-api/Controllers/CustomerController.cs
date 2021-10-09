using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        [Route("create-booking")]
        [Authorize(Roles = "Customer")]
        public IActionResult CreateBooking()
        {

            return Ok();
        }
    }
}
