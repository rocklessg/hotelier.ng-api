using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ManagerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<IEnumerable<HotelBasicDto>>> GetAllHotelsAsync(string managerId);
        Task<Response<string>> SoftDeleteManagerAsync(string managerId);
        Task<Response<ManagerResponseDto>> AddManagerAsync(ManagerDto manager);
        
    }
}
