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


        

        
    }
}