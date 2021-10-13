using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;


        public AdminController(IHotelStatisticsService hotelStatisticsService, IAdminService adminService, ILogger<AdminController> logger)
        {
            _hotelStatisticsService = hotelStatisticsService;
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet("get-statistics/manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetManagerStatistics(string managerId)
        {
            _logger.LogInformation($"About Getting Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetManagerStatistics(managerId);
            _logger.LogInformation($"Gotten Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{managerId}/statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHotelManagerStatistics(string managerId)
        {
            _logger.LogInformation($"About Getting Hotel Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetHotelManagerStatistics(managerId);

            _logger.LogInformation($"Gotten Hotel Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{managerId}/transaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHotelManagerTransactions([FromRoute] string managerId, [FromQuery] Paginator paginator)
        {
            _logger.LogInformation($"About Getting Hotel Manager Transaction for {managerId}");

             var result = await _adminService.GetManagerTransactionsAsync(managerId, paginator);

            _logger.LogInformation($"Gotten Hotel Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }
    }
}
