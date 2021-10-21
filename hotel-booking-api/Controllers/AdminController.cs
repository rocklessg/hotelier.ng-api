using hotel_booking_core.Interfaces;
using hotel_booking_dto;
<<<<<<< HEAD
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ILogger = Serilog.ILogger;
using hotel_booking_dto.commons;
=======
using static hotel_booking_utilities.Pagination.Paginator;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_utilities.Pagination;
using hotel_booking_dto.commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
>>>>>>> reviews

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
<<<<<<< HEAD
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpGet("{managerId}/transaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetHotelManagerTransactions([FromRoute] string managerId, [FromQuery] Paging paging, [FromQuery] TransactionFilter filter)
        {
            _logger.LogInformation($"Retrieveing Getting Hotel Manager Transaction for {managerId}");

            var result = await _adminService.GetManagerTransactionsAsync(managerId, paging, filter);

            _logger.LogInformation($"Retrieved Hotel Manager Transaction for {managerId}");
            return StatusCode(result.StatusCode, result);
        }
=======

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("transactions")]
          [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Response<PageResult<IEnumerable<TransactionResponseDto>>>>> GetAllTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await _adminService.GetAllTransactions(filter);
            return Ok(response);
        }

>>>>>>> reviews
    }
}