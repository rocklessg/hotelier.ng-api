using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_core.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using hotel_booking_core.Interface;
using System.Security.Claims;
using hotel_booking_dto.CustomerDtos;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        public CustomerController(IConfiguration config, IImageService imageService, IUserService userService, ICustomerService customerService)
        {
            _config = config;
            _imageService = imageService;
            _userService = userService;
            _customerService = customerService;

        }


        [HttpPut("update-customer" )]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Response<string>>> Update([FromBody] UpdateAppUserDto updateAppUser)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            
            var result = await _userService.UpdateAppUser(userId, updateAppUser);
            return StatusCode(result.StatusCode, result);


        }

        
        [HttpPatch("update-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateUserImage([FromForm] AddImageDto imageDto)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _userService.UpdateUserPhoto(imageDto, userId);
            return StatusCode(result.StatusCode, result);
        }


    }
}
