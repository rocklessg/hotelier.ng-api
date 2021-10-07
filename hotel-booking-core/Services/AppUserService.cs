using AutoMapper;
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


namespace hotel_booking_core.Services
{
    public class AppUserService : IAppUserService
    {
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

