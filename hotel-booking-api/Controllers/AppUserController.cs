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

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        private readonly IAppUserService _AppUserService;
        public AppUserController(IConfiguration config, IImageService imageService, IAppUserService AppUserService)
        {
            _config = config;
            _imageService = imageService;
            _AppUserService = AppUserService;

        }


<<<<<<< HEAD
        [Authorize]
        [HttpPatch("update")]
=======
        [HttpPut("updateAppUser")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //[Authorize(Roles = "Regular")]
        public async Task<ActionResult<Response<string>>> Update(string id, [FromBody] UpdateAppUserDto updateAppUser)
        {

            //var id = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _AppUserService.UpdateAppUser(id, updateAppUser);
            return StatusCode(result.StatusCode, result);


        }
        [HttpPatch("updateImage")]

>>>>>>> origin/reviews
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserImage([FromForm] AddImageDto imageDto)
        {


            var upload = await _imageService.UploadAsync(imageDto.Image);

            string url = upload.Url.ToString();

            //auth user id      
            string customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _AppUserService.UpdateCustomerPhoto(customerId, url);
            return StatusCode(result.StatusCode, result);

        }


    }
}
