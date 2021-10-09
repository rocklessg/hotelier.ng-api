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
    public class AdminController : ControllerBase
    {
        private readonly IManagerStatistics managerStatistics;
        private readonly ILogger<AdminController> _logger;


        public AdminController(IManagerStatistics managerStatistics, ILogger<AdminController> logger)
        {
            this.managerStatistics = managerStatistics;
            _logger = logger;
        }

        [HttpGet("get-statistics/manager")]
        public async Task<IActionResult> GetManagerStatistics(string managerId)
        {
            _logger.LogInformation($"Manager Statistics for {managerId}");

            var result = await managerStatistics.GetManagerStatistics(managerId);
            return Ok(result);
        }
    }
}
