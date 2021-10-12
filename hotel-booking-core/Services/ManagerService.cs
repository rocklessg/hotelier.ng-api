using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
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

        public async Task<string> AddManagerRequest(ManagerRequestDto managerRequest)
        {
            var addManager = _mapper.Map<ManagerRequest>(managerRequest);
            addManager.Token = _tokenGenerator.GenerateToken(addManager);
            await _unitOfWork.ManagerRequest.InsertAsync(addManager);
            await _unitOfWork.Save();
            return "allos";
        }
    }
}
