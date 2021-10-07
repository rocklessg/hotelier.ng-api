using hotel_booking_core.Interface;
using hotel_booking_core.Services;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
=======
>>>>>>> 5621354790f361918a3273e3d9b553446291e926
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



        
    }
}
