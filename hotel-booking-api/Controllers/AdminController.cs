using hotel_booking_core.Interfaces;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]/addManager")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;


        public AdminController(ILogger logger, IManagerService managerService, UserManager<AppUser> userManager)
        {
            _managerService = managerService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddManager([FromBody] ManagerResponseDto managerResponseDto)
        {
            var loggings = await _userManager.GetUserAsync(User);
            var result = await _managerService.AddManagerAsync(loggings.Id, managerResponseDto);
            return StatusCode(result.StatusCode, result);
        }

    }
}
