using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ManagerDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public ManagerService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Response<ManagerResponseDto>> AddManagerAsync(ManagerDto managerDto)
        {
            var checkManager = await _unitOfWork.Managers.CheckManagerAsync(managerDto.BusinessEmail);
            if (checkManager == null)
            {
                var appUser = new AppUser
                {

                    FirstName = managerDto.FirstName,
                    LastName = managerDto.LastName,
                    Gender = managerDto.Gender,
                    Age = managerDto.Age,
                    Email = managerDto.BusinessEmail,
                    UserName = managerDto.FirstName + managerDto.LastName

                };


                var manager = _mapper.Map<Manager>(managerDto);
                var result = await _userManager.CreateAsync(appUser, managerDto.Password);
                manager.AppUserId = appUser.Id;

                await _unitOfWork.Managers.InsertAsync(manager);
                await _unitOfWork.Save();
                if (result.Succeeded)
                {
                    var managerResponse = _mapper.Map<ManagerResponseDto>(manager);

                    var response = new Response<ManagerResponseDto>()
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Succeeded = true,
                        Data = managerResponse,
                        Message = $"{manager.CompanyName} hotel with ID: {manager.AppUserId}: registered successfully"
                    };
                    return response;
                }
                return Response<ManagerResponseDto>.Fail("Registration Failed", StatusCodes.Status404NotFound);
            }
            return Response<ManagerResponseDto>.Fail("User already exist. Please register a new manager", StatusCodes.Status404NotFound);
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
    }
}
