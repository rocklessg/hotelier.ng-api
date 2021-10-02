using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models.Mail;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmailController : Controller
    {
        private readonly IMailService mailService;
        private readonly IAuthenticationService _authService;

        public EmailController(IMailService mailService, IAuthenticationService authenticationService)
        {
            this.mailService = mailService;
            _authService = authenticationService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            var result = await _authService.ConfirmEmail(model);
            return StatusCode(result.StatusCode, result);
        }

    }
}
