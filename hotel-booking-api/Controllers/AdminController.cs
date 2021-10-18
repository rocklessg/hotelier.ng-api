using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using static hotel_booking_utilities.Pagination.Paginator;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_utilities.Pagination;
using hotel_booking_dto.commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("transactions")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await _adminService.GetAllTransactions(filter);
            return Ok(response);
        }

    }
}