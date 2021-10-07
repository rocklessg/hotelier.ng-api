using hotel_booking_dto;
using hotel_booking_dto.CustomerDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public interface IAppUserService
    {
        Task<Response<UpdateUserImageDto>> UpdateCustomerPhoto(string customerId, string url);
    }
}