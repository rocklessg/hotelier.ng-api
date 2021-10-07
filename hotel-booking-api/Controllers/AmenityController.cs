using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {

        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService AmenityService)
        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = AmenityService;
            _amenityService = amenityService;
        }

        [HttpPut("update-amenity")]
        public ActionResult<Response<UpdateAmenityDto>> UpdateAmenity(string id, [FromBody]UpdateAmenityDto update)
        [HttpGet("{hotelId}")]

        public IActionResult GetAmenityByHotelId(string hotelId)
        {
            try
            {
                var result = _amenityService.GetAmenityByHotelId(hotelId);
                return StatusCode(result.StatusCode, result);
            var response =  _amenityService.UpdateAmenity(id, update);
            return Ok(response);
            }
            catch (Exception ex)

        [HttpPost("add-amenity")]
        public async Task<ActionResult> AddAmenity(string id,[FromBody] AddAmenityRequestDto amenity)
            {
                return BadRequest(ex);
            }
            var response = await _amenityService.AddAmenity(id, amenity);
            return Ok(response);
        }
    }
}
