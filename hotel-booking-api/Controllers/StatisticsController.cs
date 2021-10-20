using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ILogger = Serilog.ILogger;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly ILogger _logger;
        public StatisticsController(IHotelStatisticsService hotelStatisticsService, ILogger logger)
        {    _hotelStatisticsService = hotelStatisticsService;
            _logger = logger;
        }

        [HttpGet("get-statistics/admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminStatistics()
        {
            _logger.Information($"About Getting Admin Statistics");

            var result = await _hotelStatisticsService.GetAdminStatistics();
            _logger.Information($"Gotten Admin Statistics");
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{managerId}/hotelManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]/*
        [Authorize(Roles = "Manager")]*/
        public async Task<IActionResult> GetHotelManagerStatistics(string managerId)
        {
            _logger.Information($"About Getting Hotel Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetHotelManagerStatistics(managerId);

            _logger.Information($"Gotten Hotel Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }        
    }
}
