using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;

namespace hotel_booking_core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly IMailService _mailService;
        

        public AuthenticationService(UserManager<AppUser> userManager,
            IMailService mailService, IMapper mapper, ITokenGeneratorService tokenGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
            _mailService = mailService;
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
            var response = new Response<LoginResponseDto>();

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
            var response = new Response<string>();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);



                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Customer);

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = $"http://www.example.com/confirmEmail/{token}/{user.Email}";
                        var mailRequest = new MailRequest()
                        {
                            Subject = "Confirm Your Registration",
                            Body = confirmationLink,
                            ToEmail = model.Email
                        };

                        var emailResult = await _mailService.SendEmailAsync(mailRequest); //Sends confirmation link to users email

                        if (emailResult)
                        {
                            response.StatusCode = (int)HttpStatusCode.Created;
                            response.Succeeded = true;
                            response.Data = user.Id;
                            response.Message = "User created successfully! Please check your mail to verify your account.";
                            transaction.Complete();
                            return response;
                        }

                    }

                    response.Message = GetErrors(result);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Succeeded = false;
                    transaction.Complete();
                    return response;
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Succeeded = false;
                    response.Message = "Registration failed. Please try again";
                    return response;
                }
            };
            
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
                response.StatusCode = (int)HttpStatusCode.BadRequest;
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
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }
    }
}
