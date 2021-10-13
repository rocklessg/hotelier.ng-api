using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController( IAdminService adminService)
        { 
            _adminService = adminService;
        }

        [HttpGet("get-all-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var response =await  _adminService.GetAllTransactions();
            return Ok(response);
        }
    }
}
