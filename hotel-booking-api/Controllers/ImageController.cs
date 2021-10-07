using hotel_booking_core.Interface;
using hotel_booking_core.Services;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
<<<<<<< HEAD

=======
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
=======
>>>>>>> 5621354790f361918a3273e3d9b553446291e926
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
      
        public ImageController(IConfiguration config, IImageService imageService)
        {
            _config = config;
            _imageService = imageService;
          

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] AddImageDto imageDto)
        {
            try
            {
                var upload = await _imageService.UploadAsync(imageDto.Image);
                var result = new ImageAddedDto()
                {
                    PublicId = upload.PublicId,
                    Url = upload.Url.ToString()

                };
                //var x = "";

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


<<<<<<< HEAD
       /* [HttpPatch]
        [Authorize("customer")]
        public async Task<IActionResult> UploadImage([FromForm] AddImageDto imageDto)
        {
            try
            {
                var response = string.Empty;
                var upload = await _imageService.UploadAsync(imageDto.Image);
               
                string url = upload.Url.ToString();
                string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userRepository.UploadImage(userId, url);
                if (result)
                {
                    response += "Image successfully added";
                }
                return Ok(response);
            }

            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }*/

=======
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10

        
    }
}
