using hotel_booking_dto;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using hotel_booking_models.Cloudinary;
using hotel_booking_utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<string>> UpdateCustomer(string CustomerId, UpdateCustomerDto updateCustomer);
        Task<Response<UpdateUserImageDto>> UpdatePhoto(AddImageDto imageDto, string userId);
        List<GetUsersResponseDto> GetAllCustomers(Paginator pagenator);
    }
}