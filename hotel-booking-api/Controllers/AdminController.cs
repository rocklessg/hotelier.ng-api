using hotel_booking_core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly ILogger<AdminController> _logger;


        public AdminController(IHotelStatisticsService hotelStatisticsService, ILogger<AdminController> logger)
        {
            _hotelStatisticsService = hotelStatisticsService;
            _logger = logger;
        }

        [HttpGet("get-statistics/manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetManagerStatistics(string managerId)
        {
            _logger.LogInformation($"About Getting Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetManagerStatistics(managerId);
            _logger.LogInformation($"Gotten Manager Statistics for {managerId}");
            return Ok(result);
        }

        [HttpGet("{managerId}/statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHotelManagerStatistics(string managerId)
        {
            _logger.LogInformation($"About Getting Hotel Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetHotelManagerStatistics(managerId);

            _logger.LogInformation($"Gotten Hotel Manager Statistics for {managerId}");
            return Ok(result);
        }
    }
}
