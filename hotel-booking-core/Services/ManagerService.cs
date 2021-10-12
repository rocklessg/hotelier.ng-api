using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddManagerRequest(ManagerRequestDto managerRequest)
        {
            var addManager = _mapper.Map<ManagerRequest>(managerRequest);
            await _unitOfWork.ManagerRequest.InsertAsync(addManager);
            await _unitOfWork.Save();
            return "allos";
        }
    }
}
