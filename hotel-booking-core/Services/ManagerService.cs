using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly IMailService _mailService;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork,
            ITokenGeneratorService tokenGenerator, IMailService mailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _mailService = mailService;

        }

        public async Task<Response<string>> AddManagerRequest(ManagerRequestDto managerRequest)
        {
            var getManager = await _unitOfWork.ManagerRequest.GetHotelManager(managerRequest.Email);

            if (getManager == null)
            {
                var addManager = _mapper.Map<ManagerRequest>(managerRequest);
                addManager.Token = _tokenGenerator.GenerateToken(addManager);
                await _unitOfWork.ManagerRequest.InsertAsync(addManager);
                await _unitOfWork.Save();

                return new Response<string>
                {
                    Message = "Thank you for interest, you will get a response from us shortly",
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true
                };
            }
            return Response<string>.Fail("Email already exist", StatusCodes.Status409Conflict);
        }

        public async Task<string> SendManagerInvite(string email)
        {
            var check = await _unitOfWork.ManagerRequest.GetHotelManager(email);

            if (check != null)
            {
                var mailBody = await GetEmailBody(emailTempPath: "StaticFiles/Html/ManagerInvite.html", token: check.Token);

                var mailRequest = new MailRequest()
                {
                    Subject = "Continue Your Registration",
                    Body = mailBody,
                    ToEmail = check.Email
                };

                var result = await _mailService.SendEmailAsync(mailRequest);
                if (result)
                {

                }
            }
        }

        private static async Task<string> GetEmailBody(string emailTempPath, string token)
        {
            var link = $"http://hoteldotnetmvc.herokuapp.com/manager/registration/{token}";
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var emailBody = temp.Replace("**link**", link);
            return emailBody;
        }
    }
}
