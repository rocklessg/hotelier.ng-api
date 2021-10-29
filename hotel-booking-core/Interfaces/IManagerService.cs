using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;

using hotel_booking_dto.ManagerDtos;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<string>> ActivateManager(string managerId);
        Task<Response<IEnumerable<HotelBasicDto>>> GetAllHotelsAsync(string managerId);
        Task<Response<string>> SoftDeleteManagerAsync(string managerId);
        Task<Response<string>> AddManagerRequest(ManagerRequestDto managerRequest);
        Task<Response<bool>> SendManagerInvite(string email);
        Task<Response<IEnumerable<ManagerRequestResponseDTo>>> GetAllManagerRequest();
        Task<Response<bool>> CheckTokenExpiring(string email, string token);
        Task<Response<ManagerResponseDto>> AddManagerAsync(ManagerDto manager);
        Task<Response<string>> UpdateManager(string managerId, UpdateManagerDto updateManager);
        Task<Response<PageResult<IEnumerable<HotelManagersDto>>>> GetAllHotelManagersAsync(PagingDto paging);

    }
}
