using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
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

        public AmenityController(IAmenityService AmenityService)
        {
            _amenityService = AmenityService;
        }

        [HttpPut]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Response<UpdateAmenityDto>>> UpdateAmenity(string id, UpdateAmenityDto update)
        {
            var response = _amenityService
        }
    }
}
