using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace hotel_booking_core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly IMailService _mailService;
        private const string FilePath = "../hotel-booking-api/StaticFiles/";


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

        public async Task<Response<string>> ForgotPassword(string email)
        {
            var response = new Response<string>();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.Message = $"An email has been sent to {email} if it exists";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var actualToken = WebEncoders.Base64UrlEncode(encodedToken);

            var passwordResetLink = $"http://www.example.com/resetpassword/{actualToken}/{email}";

            var temp = await File.ReadAllTextAsync(Path.Combine(FilePath, "Html/ForgotPassword.html"));
            var newTemp = temp.Replace("**link**", passwordResetLink);
             var emailBody = newTemp.Replace("**User**", user.FirstName);

            var mailRequest = new MailRequest()
            {
                Subject = "Reset Password",
                Body = emailBody,
                ToEmail = email
            };

            var emailResult = await _mailService.SendEmailAsync(mailRequest);
            if (emailResult)
            {
                response.Succeeded = true;
                response.Message = $"An email has been sent to {email} if it exists";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }

            response.Succeeded = false;
            response.Message = "Something went wrong. Please try again.";
            response.StatusCode = 503;
            return response;
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

                        var temp = await File.ReadAllTextAsync(Path.Combine(FilePath, "Html/ConfirmEmail.html"));
                        var newTemp = temp.Replace("**link**", confirmationLink);
                        var emailBody = newTemp.Replace("**User**", user.FirstName);

                        var mailRequest = new MailRequest()
                        {
                            Subject = "Confirm Your Registration",
                            Body = emailBody,
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

        public async Task<Response<string>> ResetPassword(ResetPasswordDto model)
        {
            var response = new Response<string>();
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                response.Message = "Invalid user!";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                response.Message = "Password does not match!";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token); //Decode incoming token
            string actualToken = Encoding.UTF8.GetString(decodedToken); //Set the token to an encoded string

            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;

            var token = await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, actualToken);
            if (token)
            {
                _mapper.Map<AppUser>(model);
                var result = await _userManager.ResetPasswordAsync(user, actualToken, model.NewPassword);
                response.Succeeded = false;
                response.Data = null;
                response.Message = GetErrors(result);
                return response;
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Password has been reset successfully";
            response.Succeeded = true;
            response.Data = user.Id;
            return response;
        }

        public async Task<Response<string>> UpdatePassword(UpdatePasswordDto model)
        {
            var response = new Response<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                response.Message = "Opps! something went wrong.";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            _mapper.Map<AppUser>(model);
            response.Message = "Password has been changed successfully";
            response.Succeeded = true;
            response.Data = user.Id;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
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
