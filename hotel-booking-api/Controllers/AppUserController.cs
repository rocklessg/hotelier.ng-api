<<<<<<< HEAD
﻿using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
=======
﻿using hotel_booking_core.Interface;
using hotel_booking_core.Services;
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
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10

namespace hotel_booking_api.Controllers
{
    [ApiController]
<<<<<<< HEAD
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {

        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPut("Update")]
        
=======
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


        [HttpPatch("update")]
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
<<<<<<< HEAD
        //[Authorize(Roles = "Regular")]
        public async Task<ActionResult<Response<string>>> Update(string id, [FromBody] UpdateAppUserRequest updateAppUser)
        {

            //var id = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _appUserService.UpdateAppUser(id, updateAppUser);
            return StatusCode(result.StatusCode, result);


=======
        public async Task<IActionResult> UpdateUserImage([FromForm] AddImageDto imageDto)
        {


            var upload = await _imageService.UploadAsync(imageDto.Image);

            string url = upload.Url.ToString();
            // string customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            string customerId = "2ccd5586-51f2-444c-aa63-e13012748dfa";
            var result = await _AppUserService.UpdateCustomerPhoto(customerId, url);
            return StatusCode(result.StatusCode, result);
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10
        }
    }
}
