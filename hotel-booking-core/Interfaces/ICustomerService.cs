using hotel_booking_dto.CustomerDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public interface ICustomerService
    {
        Task<bool> UpdateCustomer(string customerId, UpdateCustomerRequest updateCustomer);
    }
}