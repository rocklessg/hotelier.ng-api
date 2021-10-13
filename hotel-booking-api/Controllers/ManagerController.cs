using hotel_booking_core.Interfaces;
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
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpPatch("deactivate/{managerId}")]
        public async Task<ActionResult> SoftDeleteAsync(string managerId)
        {
           var response = await _managerService.SoftDeleteManagerAsync(managerId);
            return StatusCode(response.StatusCode,response);
        }
    }
}
