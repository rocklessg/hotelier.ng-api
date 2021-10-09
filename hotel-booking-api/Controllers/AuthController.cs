using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hotel_booking_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthenticationService _authService;
        
        public AuthController(ILogger<AuthController> logger,
            IAuthenticationService authService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
            
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<LoginResponseDto>>> Register([FromBody] RegisterUserDto model)
        {
            _logger.LogInformation($"Registration Attempt for {model.Email}");            
            var result = await _authService.Register(model);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [AllowAnonymous]
        [HttpPatch]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<string>>> ResetPassword([FromBody] ResetPasswordDto model)
        {
            _logger.LogInformation($"Reset Password Attempt for {model.Email}");
            var result = await _authService.ResetPassword(model);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize]
        [HttpPatch]
        [Route("update-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<string>>> UpdatePassword([FromBody] UpdatePasswordDto model)
        {

            var result = await _authService.UpdatePassword(model);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<string>>> ForgotPassword(string email)
        {
            _logger.LogInformation($"Forgot Password Attempt for {email}");

            var result = await _authService.ForgotPassword(email);
            return StatusCode(result.StatusCode, result);
        }
    }
}
