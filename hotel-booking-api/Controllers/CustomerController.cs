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


        [HttpPut("update-user" )]

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
        public async Task<IActionResult> UpdateUserImage([FromForm] AddImageDto imageDto)
        {


            var upload = await _imageService.UploadAsync(imageDto.Image);

            string url = upload.Url.ToString();

            //auth user id
            
            //string customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;


            string customerId = "2ccd5586-51f2-444c-aa63-e13012748dfa";
            var result = await _userService.UpdateCustomerPhoto(customerId, url);
            return StatusCode(result.StatusCode, result);

        }


    }
}
