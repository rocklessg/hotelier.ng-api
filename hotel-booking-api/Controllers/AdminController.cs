﻿using hotel_booking_core.Interfaces;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public AdminController(IManagerService managerService)
        {
            _managerService = managerService;
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

    }
}
