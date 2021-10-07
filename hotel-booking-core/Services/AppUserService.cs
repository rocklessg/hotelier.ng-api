
using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using hotel_booking_dto.CustomerDtos;



namespace hotel_booking_core.Services
{
    public class AppUserService : IAppUserService
    {

        private readonly UserManager<AppUser> _UserManager;
        private readonly IMapper _mapper;

        public AppUserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _UserManager = userManager;
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

            var user = await _UserManager.FindByIdAsync(appUserId);
            if (user != null)
            {

                var model = _mapper.Map(updateAppUser, user);

                var result = await _UserManager.UpdateAsync(model);

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
        public async Task<Response<UpdateUserImageDto>> UpdateCustomerPhoto(string customerId, string url)
        {
            AppUser customer = await _UserManager.FindByIdAsync(customerId);
            customer.Avatar = url;

            var result = await _UserManager.UpdateAsync(customer);

            var response = new Response<UpdateUserImageDto>()
            {
                StatusCode = result.Succeeded == true ? 200 : 400,
                Succeeded = result.Succeeded == true ? true : false,
                Data = new UpdateUserImageDto { Url = url },
                Message = result.Succeeded == true ? "image upload successful" : "failed"
            };
            return response;
        }

    }
}


       
       
       
   
    

