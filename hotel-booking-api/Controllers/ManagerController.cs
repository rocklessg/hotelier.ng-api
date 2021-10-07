using hotel_booking_core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatistics;
        private readonly ILogger<ManagerController> _logger;
        public ManagerController(IHotelStatisticsService hotelStatistics, ILogger<ManagerController> logger)
        {
            _hotelStatistics = hotelStatistics;
            _logger = logger;
        }

        [HttpGet("{managerId}/statistics")]
        public async Task<IActionResult> GetHotelManagerStatistics(string managerId)
        {
            _logger.LogInformation($"Hotel Manager Statistics for {managerId}");
            var result = await _hotelStatistics.GetHotelManagerStatistics(managerId);
            return Ok(result);
        }
    }
}
