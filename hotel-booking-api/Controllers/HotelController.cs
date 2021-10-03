using hotel_booking_core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatistics;

        public HotelController(IHotelStatisticsService hotelStatistics)
        {
            _hotelStatistics = hotelStatistics;
        }

       

        [HttpGet("{hotelId}/statistics")]
        public async Task<IActionResult> GetHotelStatistics(string hotelId) 
        {
            var hotel = await _hotelStatistics.GetHotelStatistics(hotelId);
            return Ok(hotel);
        }
    }
}
