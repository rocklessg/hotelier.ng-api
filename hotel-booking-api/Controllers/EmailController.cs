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

        public EmailController(IMailService mailService)
        {
            this.mailService = mailService;
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

    }
}
