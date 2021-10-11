using hotel_booking_dto;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models.Cloudinary;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<string>> UpdateCustomer(string CustomerId, UpdateCustomerDto updateCustomer);
        Task<Response<UpdateUserImageDto>> UpdatePhoto(AddImageDto imageDto, string userId);
    }
}