using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
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
            return Response<ManagerResponseDto>.Fail("There is no such email", StatusCodes.Status404NotFound);
        }
    }
}
