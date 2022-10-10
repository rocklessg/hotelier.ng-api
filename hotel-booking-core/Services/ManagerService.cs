using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ManagerService(IUnitOfWork unitOfWork, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<HotelBasicDto>>> GetAllHotelsAsync(string managerId)
        {
            var hotelList = await _unitOfWork.Managers.GetAllHotelsForManagerAsync(managerId);
            var hotelListDto = _mapper.Map<IEnumerable<HotelBasicDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDto>>(StatusCodes.Status200OK, true, "hotels for manager", hotelListDto);
            return response;
        }

        public async Task<Response<string>> SoftDeleteManagerAsync(string managerId)
        {
            Manager manager = await _unitOfWork.Managers.GetManagerAsync(managerId);
            Response<string> response = new();

            if(manager != null)
            {
                if(manager.AppUser.IsActive == true)
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
    }
}
