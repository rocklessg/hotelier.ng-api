﻿using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.ManagerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Route("AddManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddManager([FromBody] ManagerDto managerDto)
        {
            var result = await _managerService.AddManagerAsync(managerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> AddHotelManagerRequest([FromBody]ManagerRequestDto managerRequestDto)
        {
            var newManagerRequest = await _managerService.AddManagerRequest(managerRequestDto);
            _logger.Information($"Request to join is successfully added to the database");
            return Ok(newManagerRequest);
        }

        [HttpGet]
        [Route("send-invite")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> SendManagerInvite(string email)
        {
            var sendInvite = await _managerService.SendManagerInvite(email);
            _logger.Information($"Invite has been successfully sent to {email}");
            return Ok(sendInvite);
        }

        [HttpGet]
        [Route("validate-email")]
        public async Task<IActionResult> TokenExpiring(string email, string token)
        {
            var confirmToken = await _managerService.CheckTokenExpiring(email, token);
            return Ok(confirmToken);
        }

        [HttpGet]
        [Route("getall-request")]
        public async Task<IActionResult> GetAllRequests()
        
        {
            var getAll = await _managerService.GetAllManagerRequest();
            return Ok(getAll);
           
        }
        [HttpPatch("{managerId}/deactivate")]
        public async Task<ActionResult> SoftDeleteAsync(string managerId)
        {
            var response = await _managerService.SoftDeleteManagerAsync(managerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{managerId}/activate")]
        [Authorize(Policy = Policies.Admin)]
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
