using hotel_booking_core.Interface;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; 
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        private readonly IAppUserService _AppUserService;
        public AppUserController (IConfiguration config, IImageService imageService, IAppUserService AppUserService)
        {
            _config = config;
            _imageService = imageService;
            _AppUserService = AppUserService;

        }


        [Authorize]
        [HttpPatch("update")]
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
