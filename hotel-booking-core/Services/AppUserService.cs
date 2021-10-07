using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Implementation;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using hotel_booking_dto;
using Microsoft.AspNetCore.Identity;
using hotel_booking_core.Interface;

namespace hotel_booking_core.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppUserRepository _customerRepository;
        private readonly UserManager<AppUser> _UserManager;

        public AppUserService(IUnitOfWork unitOfWork, IAppUserRepository customerRepository,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _UserManager = userManager;
        }
        public async Task <Response<UpdateUserImageDto>> UpdateCustomerPhoto(string customerId,string url)
        {
            AppUser customer = await _UserManager.FindByIdAsync(customerId);
            customer.Avatar = url;
            
            var result = await _UserManager.UpdateAsync(customer);

            var response = new Response<UpdateUserImageDto>()
            {
                StatusCode = result.Succeeded==true? 200 : 400,
                Succeeded = result.Succeeded == true ? true : false,
                Data = new UpdateUserImageDto { Url = url },
                Message = result.Succeeded == true ? "image upload successful" : "failed"
            };
            return response;
        }
       
    }

    }

    
