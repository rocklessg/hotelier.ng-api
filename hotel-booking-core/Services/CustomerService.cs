using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Identity;
using hotel_booking_models;
using System.Transactions;
using hotel_booking_models.Cloudinary;
using hotel_booking_core.Interface;

namespace hotel_booking_core.Services
{
    public class CustomerService :  ICustomerService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly UserManager<AppUser> _userManager;

        public CustomerService(IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _imageService = imageService;
        }

        public async Task<Response<string>> UpdateCustomer(string customerId, UpdateCustomerDto updateCustomer)
        {
            var response = new Response<string>();

            var customer =  _unitOfWork.Customers.GetCustomer(customerId);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (customer != null)
                {
                    // Update user details in AspNetAppUser table
                    var user = await _userManager.FindByIdAsync(customerId);

                    var userUpdateResult = await UpdateUser(user, updateCustomer);

                    if (userUpdateResult.Succeeded)
                    {
                        customer.CreditCard = updateCustomer.CreditCard;
                        customer.Address = updateCustomer.Address;
                        customer.State = updateCustomer.State;

                        _unitOfWork.Customers.Update(customer);
                        await _unitOfWork.Save();

                        response.Message = "Update Successful";
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Succeeded = true;
                        response.Data = customerId;
                        transaction.Complete();
                        return response;
                    }

                    transaction.Dispose();
                    response.Message = "Something went wrong, when updating the AppUser table. Please try again later";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Succeeded = false;
                    return response;
                }

                response.Message = "Customer Not Found";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Succeeded = false;
                transaction.Complete();
                return response;
            }
                
        }

        public async Task<Response<UpdateUserImageDto>> UpdatePhoto(AddImageDto imageDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var upload = await _imageService.UploadAsync(imageDto.Image);
                string url = upload.Url.ToString();
                user.Avatar = url;
                user.PublicId = upload.PublicId;
                await _userManager.UpdateAsync(user);

                return Response<UpdateUserImageDto>.Success("image upload successful", new UpdateUserImageDto { Url = url });
            }
            return Response<UpdateUserImageDto>.Fail("user not found");

        }

        private async Task<IdentityResult> UpdateUser(AppUser user, UpdateCustomerDto model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Age = model.Age;

            return await _userManager.UpdateAsync(user);
        }

    }
}