using hotel_booking_core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {

        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet("{hotelId}")]

        public IActionResult GetAmenityByHotelId(string hotelId)
        {
            try
            {
                var result = _amenityService.GetAmenityByHotelId(hotelId);
                return Ok(result);
            }
            catch (ArgumentException errors)
            {
                return BadRequest(errors.Message);
            }
        }
    }
}
