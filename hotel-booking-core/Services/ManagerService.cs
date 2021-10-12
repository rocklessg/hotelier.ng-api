using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGeneratorService _tokenGenerator;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork,
            ITokenGeneratorService tokenGenerator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
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
    }
}
