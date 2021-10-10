
using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.AppUserDto;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_core.Interface;
using Microsoft.AspNetCore.Mvc;
using hotel_booking_models.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace hotel_booking_core.Services
{
    public class AppUserService : IAppUserService
    {

        private readonly UserManager<AppUser> _UserManager;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public AppUserService(UserManager<AppUser> userManager, IMapper mapper, IImageService imageService)
        {
            _UserManager = userManager;
            _mapper = mapper;
            _imageService = imageService;
        }


        /// <summary>
        /// A method to update an AppUser
        /// </summary>
        /// <param name="appUserId"></param>
        /// <param name="updateAppUser"></param>
        /// <returns>ask<Response<string>></returns>
        public async Task<Response<string>> UpdateAppUser(string appUserId, UpdateAppUserDto updateAppUser)
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

        }
        public async Task<Response<UpdateUserImageDto>> UpdateUserPhoto([FromForm] AddImageDto imageDto, string userId)
        {

            AppUser user = await _UserManager.FindByIdAsync(userId);


            if (user is not null)
            {
                var upload = await _imageService.UploadAsync(imageDto.Image);
                string url = upload.Url.ToString();
                user.Avatar = url;
                user.PublicId = upload.PublicId;
                await _UserManager.UpdateAsync(user);

                return Response<UpdateUserImageDto>.Success("image upload successful", new UpdateUserImageDto { Url = url}); 
            }
            return Response<UpdateUserImageDto>.Fail("user not found");
            
        }
    }
}









