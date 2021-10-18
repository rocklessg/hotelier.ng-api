using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_models.Mail;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly ILogger _logger;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork, 
            IMailService mailService, ILogger logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _logger = logger;

        }

        public async Task<Response<string>> SoftDeleteManagerAsync(string managerId)
        {
            Manager manager = await _unitOfWork.Managers.GetManagerAsync(managerId);
            Response<string> response = new();

            if (manager != null)
            {
                if (manager.AppUser.IsActive == true)
                {
                    manager.AppUser.IsActive = false;
                    _unitOfWork.Managers.Update(manager);
                    await _unitOfWork.Save();

                    response.Message = $"Manager with {manager.AppUser.Id} has been deactivated successfully.";
                    response.StatusCode = (int)HttpStatusCode.Created;
                    response.Succeeded = true;

                    return response;
                }

                response.Message = $"Attention, manager with {manager.AppUser.Id} is already inactive.";
                response.StatusCode = (int)HttpStatusCode.AlreadyReported;
                response.Succeeded = false;

                return response;

            }
            response.Message = $"Sorry, user with {managerId} is not a manager.";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;

            return response;
    } 

        public async Task<Response<string>> AddManagerRequest(ManagerRequestDto managerRequest)
        {
            var getManager = await _unitOfWork.ManagerRequest.GetHotelManagerByEmail(managerRequest.Email);
            var getUser = await _unitOfWork.Managers.GetAppUserByEmail(managerRequest.Email);

            if (getUser == null)
            {
                if (getManager == null)
                {
                    var addManager = _mapper.Map<ManagerRequest>(managerRequest);
                    addManager.Token = Guid.NewGuid().ToString();
                    await _unitOfWork.ManagerRequest.InsertAsync(addManager);
                    await _unitOfWork.Save();

                    return new Response<string>
                    {
                        Message = "Thank you for your interest, you will get a response from us shortly",
                        StatusCode = StatusCodes.Status200OK,
                        Succeeded = true
                    };
                }
                return Response<string>.Fail("Email already exist", StatusCodes.Status409Conflict);
            }
            return Response<string>.Fail("This Email is a registered user", StatusCodes.Status409Conflict);
        }

        public async Task<Response<string>> SendManagerInvite(string email)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var check = await _unitOfWork.ManagerRequest.GetHotelManagerByEmail(email);
            var getUser = await _unitOfWork.Managers.GetAppUserByEmail(email);
            if (getUser == null)
            { 
                if (check != null)
                {
                    var newGuid = Guid.Parse(check.Token);
                    var mailBody = await GetEmailBody(emailTempPath: "StaticFiles/Html/ManagerInvite.html", token: Encode(newGuid), email);

                    var mailRequest = new MailRequest()
                    {
                        Subject = "Request Approved",
                        Body = mailBody,
                        ToEmail = check.Email
                    };

                    var result = await _mailService.SendEmailAsync(mailRequest);
                    if (result)
                    {
                        check.ExpiresAt = DateTime.UtcNow.AddHours(24);
                        _unitOfWork.ManagerRequest.Update(check);
                    await _unitOfWork.Save();

                        _logger.Information("Mail sent successfully");
                        return new Response<string>
                        {
                            Data = check.Id,
                            StatusCode = StatusCodes.Status200OK,
                            Message = $"Message successfully sent",
                            Succeeded = true
                        };
                    }
                    _logger.Information("Mail service failed");
                    transaction.Dispose();
                    return Response<string>.Fail("Mail service failed", StatusCodes.Status503ServiceUnavailable);
                }
                transaction.Complete();
                _logger.Information("Invalid email address");
                return Response<string>.Fail("Invalid email address", StatusCodes.Status404NotFound);
            }
            return Response<string>.Fail("This email is a registered user", StatusCodes.Status404NotFound);
        }

        public async Task<Response<bool>> CheckTokenExpiring(string email, string token)
        {
            var managerRequest = await _unitOfWork.ManagerRequest.GetHotelManagerByEmailToken(email, token);
            var getUser = await _unitOfWork.Managers.GetAppUserByEmail(email);

            if (managerRequest != null)
            {
                if (getUser == null)
                {
                    var expired = managerRequest.ExpiresAt < DateTime.Now;
                    if (expired)
                    {
                        var resendMail = await SendManagerInvite(email);
                        if (resendMail.Succeeded)
                        {
                            return Response<bool>.Fail("Link has expired, a new link has been sent", StatusCodes.Status408RequestTimeout);
                        }
                        return Response<bool>.Fail("Weak or no internet access, please try again", StatusCodes.Status408RequestTimeout);
                    }
                    return Response<bool>.Success("Redirecting to registration page", true, StatusCodes.Status200OK);
                }
                return Response<bool>.Fail("This User is a registered already", StatusCodes.Status409Conflict);
            }
            return Response<bool>.Fail("Invalid email or token", StatusCodes.Status404NotFound);
        }

        public async Task<Response<IEnumerable<ManagerRequestResponseDTo>>> GetAllManagerRequest()
        {
            var getAllManagersRequest = await _unitOfWork.ManagerRequest.GetManagerRequest();
            
            var mapResponse = _mapper.Map<List<ManagerRequestResponseDTo>>(getAllManagersRequest);

            return Response<IEnumerable<ManagerRequestResponseDTo>>
                .Success("All manager requests", mapResponse, StatusCodes.Status200OK); 
        }

        private static async Task<string> GetEmailBody(string emailTempPath, string token, string email)  
        {
            var link = $"https://hoteldotnet.herokuapp.com/api/Manager/validate-email?{email}&token={token}";
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var emailBody = temp.Replace("**link**", link);
            return emailBody;
        }

        public static string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
            }

        public static Guid Decode(string value)
        {
            value = value.Replace("_", "/").Replace("-", "+");
            var buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }
    }
}
