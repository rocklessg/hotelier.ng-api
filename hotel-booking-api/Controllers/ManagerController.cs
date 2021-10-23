using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPatch("{managerId}/deactivate")]
        public async Task<ActionResult> SoftDeleteAsync(string managerId)
        {
            var response = await _managerService.SoftDeleteManagerAsync(managerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{managerId}/activate")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Response<string>>> ActivateManager(string managerId)
        {
            var response = await _managerService.ActivateManager(managerId);
            return Ok(response);
        }

        [HttpGet]
        [Route("{managerId}/Hotels")]
        public async Task<IActionResult> GetAllHotels(string managerId)
        {
            var response = await _managerService.GetAllHotelsAsync(managerId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
