using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {

        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPut("Update")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Regular")]
        public async Task<ActionResult<Response<string>>> Update(string id, [FromBody] UpdateAppUserRequest updateAppUser)
        {

            //var id = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _appUserService.UpdateAppUser(id, updateAppUser);
            return StatusCode(result.StatusCode, result);


        }
    }
}
