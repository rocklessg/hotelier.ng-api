using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using hotel_booking_dto;

namespace hotel_booking_core.Services
{
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;
        IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = _unitOfWork.Customers;
        }


        #region 
        /// <summary>
        /// A method to update a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="updateCustomer"></param>
        /// <returns>Task<bool></returns>
        public async Task<Response<UpdateCustomerResponseDto>> UpdateCustomer(string customerId, UpdateCustomerRequest updateCustomer)
        {
            Customer customer = await _customerRepository.FindAsync(customerId);
            if (customer != null)
            {
                customer.AppUser.FirstName = string.IsNullOrWhiteSpace(updateCustomer.FirstName) ? customer.AppUser.FirstName : updateCustomer.FirstName;
                customer.AppUser.LastName = string.IsNullOrWhiteSpace(updateCustomer.LastName) ? customer.AppUser.LastName : updateCustomer.LastName;
                customer.AppUser.Email = string.IsNullOrWhiteSpace(updateCustomer.EmailAddress) ? customer.AppUser.Email : updateCustomer.EmailAddress;
                customer.AppUser.PhoneNumber = string.IsNullOrWhiteSpace(updateCustomer.PhoneNumber) ? customer.AppUser.PhoneNumber : updateCustomer.PhoneNumber;
                customer.AppUser.UserName = string.IsNullOrWhiteSpace(updateCustomer.UserName) ? customer.AppUser.UserName : updateCustomer.LastName;
                customer.AppUser.Gender = string.IsNullOrWhiteSpace(updateCustomer.Gender) ? customer.AppUser.Gender : updateCustomer.Gender;
                customer.AppUser.Age = updateCustomer.Age < 1 ? customer.AppUser.Age : updateCustomer.Age;

                _customerRepository.Update(customer);
                await _unitOfWork.Save();


            }
            var response = new Response<UpdateCustomerResponseDto>
            {
                Message = customer != null ? "Update Successful" : "failed",
                StatusCode = customer != null ? 200 : 400,
                Succeeded = customer != null ? true : false,
                Data = new UpdateCustomerResponseDto { Id = customerId }
            };

            return response;
            #endregion

        }
    }
}