using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly ILogger _logger;


        public ManagerController(ILogger logger, IManagerService managerService)
        {
            _managerService = managerService;
            _logger = logger;
        }

        [HttpPost]
        [Route("join")]
        public async Task<IActionResult> AddHotelManagerRequest([FromBody]ManagerRequestDto managerRequestDto)
        {
            var newManagerRequest = await _managerService.AddManagerRequest(managerRequestDto);
            _logger.Information($"Request to join is successfully added to the database");
            return Ok(newManagerRequest);
        }

        [HttpGet]
        [Route("send-invite")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendManagerInvite(string email)
        {
            var sendInvite = await _managerService.SendManagerInvite(email);
            _logger.Information($"Invite has been successfully sent to {email}");
            return Ok(sendInvite);
        }
    }
}
