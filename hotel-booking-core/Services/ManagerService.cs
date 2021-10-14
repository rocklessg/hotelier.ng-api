using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<Response<ManagerResponseDto>> AddManagerAsync(string managerId, ManagerResponseDto managerDto)
        {

            Manager manager = _mapper.Map<Manager>(managerDto);

            manager.AppUserId = managerId;

            await _unitOfWork.Managers.InsertAsync(manager);
            await _unitOfWork.Save();

            var managerResponse = _mapper.Map<ManagerResponseDto>(manager);

            var response = new Response<ManagerResponseDto>()
            {
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true,
                Data = managerResponse,
                Message = $"{manager.CompanyName} with id {manager.AppUserId} has been added"
            };
            return response;
        }
    }
}
