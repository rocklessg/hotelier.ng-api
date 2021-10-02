using System.Threading.Tasks;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models;
using hotel_booking_models.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hotel_booking_api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IMailService _mailService;
        public AuthController(ILogger<AuthController> logger, IMailService mailService,
            IAuthenticationService authService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<LoginResponseDto>>> Register([FromBody] RegisterUserDto model)
        {
            _logger.LogInformation($"Registration Attempt for {model.Email}");            
            var result = await _authService.Register(model);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(await _userManager.FindByIdAsync(result.Data));
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Email", new { token, email = model.Email }, Request.Scheme);
                MailRequest mailRequest = new()
                {
                    Subject = "Confirm Your Registration",
                    Body = confirmationLink,
                    ToEmail = model.Email
                    
                };
                await _mailService.SendEmailAsync(mailRequest);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<string>>> Login([FromBody] LoginDto model)
        {
            _logger.LogInformation($"Login Attempt for {model.Email}");
            var result = await _authService.Login(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Route("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<string>>> ConfirmEmail([FromBody] ConfirmEmailDto model)
        {
            var result = await _authService.ConfirmEmail(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
