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
        public Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<LoginResponseDto>> Login(LoginDto model)
        {
            Response<LoginResponseDto> response = new();
            if (!await ValidateUser(model))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = "Email or password is incorrect";
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
            response.Succeeded = true;
            response.Data = result;
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

            foreach (var err in result.Errors)
            {
                response.Message += err.Description + ", ";
            }
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

        public async Task<bool> ValidateUser(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            return (user != null && await _userManager.CheckPasswordAsync(user, model.Password));
        }
    }
}
