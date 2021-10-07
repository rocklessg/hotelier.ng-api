using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hotel_booking_core.Services;
using hotel_booking_dto.CustomerDtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using hotel_booking_dto;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.AppUserDto;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService; 
        }

        [HttpPut("Update")]
        //[Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Regular")]
        public ActionResult<Response<UpdateAppUserResponseDto>> Update([FromBody]UpdateCustomerRequest updateCustomer)
        {
           
                /*var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = _customerService.UpdateCustomer (userId, updateCustomer);*/
                var result = _customerService.UpdateCustomer("2ccd5586-51f2-444c-aa63-e13012748dfa", updateCustomer);
               return Ok(result);

            
           
        }
    }
}
