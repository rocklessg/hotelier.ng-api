using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_core.Interfaces;

namespace hotel_booking_core.Services
{
    public class CustomerService : ICustomerService
    {
        IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }       

        
    }
}