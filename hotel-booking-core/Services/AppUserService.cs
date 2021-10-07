<<<<<<< HEAD
﻿using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

=======
﻿using System;
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
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10

namespace hotel_booking_core.Services
{
    public class AppUserService : IAppUserService
    {
<<<<<<< HEAD
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AppUserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


        #region 
        /// <summary>
        /// A method to update a customer
        /// </summary>
        /// <param name="appUserId"></param>
        /// <param name="updateAppUser"></param>
        /// <returns>Task<bool></returns>
        public async Task<Response<string>> UpdateAppUser(string appUserId, UpdateAppUserRequest updateAppUser)
        {
            var response = new Response<string>();

            var user = await _userManager.FindByIdAsync(appUserId);
            if (user != null)
            {
                //user.FirstName = string.IsNullOrWhiteSpace(updateAppUser.FirstName) ? user.FirstName : updateAppUser.FirstName;
                //user.LastName = string.IsNullOrWhiteSpace(updateAppUser.LastName) ? user.LastName : updateAppUser.LastName;
                //user.PhoneNumber = string.IsNullOrWhiteSpace(updateAppUser.PhoneNumber) ? user.PhoneNumber : updateAppUser.PhoneNumber;
                //user.Age = updateAppUser.Age < 1 ? user.Age : updateAppUser.Age;

                var model = _mapper.Map(updateAppUser, user);

                var result = await _userManager.UpdateAsync(model);

                if (result.Succeeded)
                {
                    response.Message = "Update Successful";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Succeeded = true;
                    response.Data = user.Id;
                    return response;
                }

                response.Message = "Something went wrong. Please try again later";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Succeeded = false;
                return response;


            }

            response.Message = "Not Found";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            return response;
            #endregion

        }
    }
}

=======
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

    
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10
