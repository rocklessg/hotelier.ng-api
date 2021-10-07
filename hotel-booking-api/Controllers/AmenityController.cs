using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService AmenityService)
        {
            _amenityService = AmenityService;
        }

        [HttpPut("update-amenity")]
        public ActionResult<Response<UpdateAmenityDto>> UpdateAmenity(string id, [FromBody]UpdateAmenityDto update)
        {
            var response =  _amenityService.UpdateAmenity(id, update);
            return Ok(response);
        }

        [HttpPost("add-amenity")]
        public async Task<ActionResult> AddAmenity(string id,[FromBody] AddAmenityRequestDto amenity)
        {
            var response = await _amenityService.AddAmenity(id, amenity);
            return Ok(response);
        }
    }
}
