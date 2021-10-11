﻿using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
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
        private readonly IUnitOfWork _unitOfWork;
        private const string FilePath = "../hotel-booking-api/StaticFiles/";


        public AuthenticationService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
            IMailService mailService, IMapper mapper, ITokenGeneratorService tokenGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
            _mailService = mailService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Confirms the mail of a registered user
        /// </summary>
        /// <param name="confirmEmailDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends a reset password token to a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

            var mailBody = await GetEmailBody(user, emailTempPath: "Html/ForgotPassword.html", linkName: "reset-password", actualToken);

            var mailRequest = new MailRequest()
            {
                Subject = "Reset Password",
                Body = mailBody,
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
            response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return response;
        }       
      
        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Registers a new user and sends a confirmation link to the user's email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<string>> Register(RegisterUserDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            user.IsActive = true;
            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var mailBody = await GetEmailBody(user, emailTempPath: "Html/ConfirmEmail.html", linkName: "confirm-email", token);
                    var mailRequest = new MailRequest()
                    {
                        Subject = "Confirm Your Registration",
                        Body = mailBody,
                        ToEmail = model.Email
                    };
                    var emailResult = await _mailService.SendEmailAsync(mailRequest); //Sends confirmation link to users email
                    if (emailResult)
                    {
                        var customer = new Customer
                        {
                            AppUser = user
                        };
                        await _unitOfWork.Customers.InsertAsync(customer);
                        await _unitOfWork.Save();
                        response.StatusCode = (int)HttpStatusCode.Created;
                        response.Succeeded = true;
                        response.Data = user.Id;
                        response.Message = "User created successfully! Please check your mail to verify your account.";
                        transaction.Complete();
                        return response;
                    }
                    transaction.Dispose();
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Succeeded = false;
                    response.Message = "Registration failed. Please try again";
                    return response;
                }
                response.Message = GetErrors(result);
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Succeeded = false;
                transaction.Complete();
                return response;


            };

        }

        /// <summary>
        /// Reset the password of a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update the password of a logged in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Validates a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns true if the user exists</returns>
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
            if(!await _userManager.IsEmailConfirmedAsync(user) && user.IsActive)
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

        /// <summary>
        /// Stringify and returns all the identity errors
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Identity Errors</returns>
        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }

        /// <summary>
        /// Generates and encapsulates the link to be sent to the user in a html template.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="emailTempPath"></param>
        /// <param name="linkName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task<string> GetEmailBody(AppUser user, string emailTempPath, string linkName, string token)
        {
            var link = $"http://www.example.com/{linkName}/{token}/{user.Email}";
            var temp = await File.ReadAllTextAsync(Path.Combine(FilePath, emailTempPath));
            var newTemp =  temp.Replace("**link**", link);
            var emailBody = newTemp.Replace("**User**", user.FirstName);
            return emailBody;
        }
    }
}
