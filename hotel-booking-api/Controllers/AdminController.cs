using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Serilog;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IHotelStatisticsService _hotelStatisticsService;
        private readonly IManagerService _managerService;
        private readonly ILogger _logger;


        public AdminController(IHotelStatisticsService hotelStatisticsService, 
            ILogger logger, IManagerService managerService)
        {
            _managerService = managerService;
            _hotelStatisticsService = hotelStatisticsService;
            _logger = logger;
        }

        [HttpGet("get-statistics/manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetManagerStatistics(string managerId)
        {
            _logger.Information($"About Getting Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetManagerStatistics(managerId);
            _logger.Information($"Gotten Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{managerId}/statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHotelManagerStatistics(string managerId)
        {
            _logger.Information($"About Getting Hotel Manager Statistics for {managerId}");

            var result = await _hotelStatisticsService.GetHotelManagerStatistics(managerId);

            _logger.Information($"Gotten Hotel Manager Statistics for {managerId}");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("manager/join")]
        public async Task<IActionResult> AddHotelManagerRequest([FromBody]ManagerRequestDto managerRequestDto)
        {
            var newManagerRequest = await _managerService.AddManagerRequest(managerRequestDto);
            _logger.Information($"Request to join is successfully added to the database");
            return Ok(newManagerRequest);
        }

        [HttpGet]
        [Route("manager/send-invite")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendManagerInvite(string email)
        {
            var sendInvite = await _managerService.SendManagerInvite(email);
            _logger.Information($"Invite has been successfully sent to {email}");
            return Ok(sendInvite);
        }
    }
}
