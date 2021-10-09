using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using hotel_booking_dto;
using AutoMapper;
using hotel_booking_dto.AppUserDto;
using System.Net;

namespace hotel_booking_core.Services
{
    public class CustomerService :  ICustomerService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }

        public async Task<Response<string>> UpdateCustomer(string CustomerId, UpdateCustomerRequest updateCustomer)
        {
            var response = new Response<string>();

            var customer =  _unitOfWork.Customers.GetCustomer(CustomerId);
            if (customer != null)
            {

                //using manual mapping for now 
                customer.Address = updateCustomer.Address;
                customer.State = updateCustomer.State;
                customer.CreditCard = updateCustomer.CreditCard;


                
               // var model = _mapper.Map<Customer>(customer);
                _unitOfWork.Customers.UpdateAsync(customer);
                await _unitOfWork.Save();

                    response.Message = "Update Successful";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Succeeded = true;
                    response.Data = CustomerId;
                    return response;

            }

            response.Message = "Not Found";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            return response;
        }



    }
}