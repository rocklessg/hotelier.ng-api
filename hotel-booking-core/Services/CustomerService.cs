using System.Threading.Tasks;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_core.Interfaces;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_dto;
using AutoMapper;
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

        public async Task<Response<string>> UpdateCustomer(string CustomerId, UpdateCustomerDto updateCustomer)
        {
            var response = new Response<string>();

            var customer =  _unitOfWork.Customers.GetCustomer(CustomerId);
            if (customer != null)
            {

               
                var result = _mapper.Map(updateCustomer, customer);


                _unitOfWork.Customers.Update(result);
                await _unitOfWork.Save();

                response.Message = "Update Successful";
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = CustomerId;
                return response;

            }

            response.Message = "Customer Not Found";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = false;
            return response;
        }



    }
}