using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenGeneratorService _tokenGenerator;

        public AuthenticationService(UserManager<AppUser> userManager,
            IMapper mapper, ITokenGeneratorService tokenGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
            var response = new Response<string>();
            if(user == null)
            {
                response.Message = "User not found";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);
            if (result.Succeeded)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Email Confirmation successful";
                response.Data = user.Id;
                response.Succeeded = true;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = GetErrors(result);
            response.Succeeded = false;
            return response;
            
        }

        public Task<Response<string>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<LoginResponseDto>> Login(LoginDto model)
        {
            Response<LoginResponseDto> response = new();

            var validityResult = await ValidateUser(model);

            if (!validityResult.Succeeded)
            {
                response.Message = validityResult.Message;
                response.StatusCode = validityResult.StatusCode;
                response.Succeeded = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = new LoginResponseDto()
            {
                Id = user.Id,
                Token = await _tokenGenerator.GenerateToken(user)
            };
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Login Successfully";
            response.Data = result;
            response.Succeeded = true;
            return response;
        }

        public async Task<Response<string>> Register(RegisterUserDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            Response<string> response = new();

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                response.StatusCode = (int)HttpStatusCode.Created;
                response.Succeeded = true;
                response.Data = user.Id;
                response.Message = "User created successfully! Please check your mail to verify your account.";
                return response;
            }

            response.Message = GetErrors(result);
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            return response;
        }

        public Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> UpdatePassword(RegisterUserDto addUserDto)
        {
            throw new NotImplementedException();
        }

        private async Task<Response<bool>> ValidateUser(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var response = new Response<bool>();
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Message = "Invalid Credentials";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                response.Message = "Account not activated";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                return response;
            }
            else
            {
                response.Succeeded = true;
                return response;
            }
        }

        private static string GetErrors(IdentityResult result)
        {
            string message = string.Empty;
            foreach (var err in result.Errors)
            {
                message += err.Description + "\n";
            }
            return message;
        }
    }
}
