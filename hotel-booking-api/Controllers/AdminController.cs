using hotel_booking_core.Interfaces;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using hotel_booking_dto;
using static hotel_booking_utilities.Pagination.Paginator;
using hotel_booking_utilities.Pagination;
using hotel_booking_dto.commons;
using System.Collections.Generic;
namespace hotel_booking_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IAdminService _adminService;

        public AdminController(IManagerService managerService, IAdminService adminService)
        {
            _managerService = managerService;
            _adminService = adminService;
        }

        [HttpPost]
        [Route("AddManager")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddManager([FromBody] ManagerDto managerDto)
        {
            var result = await _managerService.AddManagerAsync(managerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Route("transactions")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Response<PageResult<IEnumerable<TransactionResponseDto>>>>> GetAllTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await _adminService.GetAllTransactions(filter);
            return Ok(response);
        }
    }
}


