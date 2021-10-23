using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<string>> ActivateManager(string managerId);
        Task<Response<IEnumerable<HotelBasicDto>>> GetAllHotelsAsync(string managerId);
        Task<Response<string>> SoftDeleteManagerAsync(string managerId);
    }
}
