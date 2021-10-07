using hotel_booking_dto;
using hotel_booking_dto.CustomerDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<UpdateCustomerResponseDto>> UpdateCustomer(string customerId, UpdateCustomerRequest updateCustomer);
    }
}