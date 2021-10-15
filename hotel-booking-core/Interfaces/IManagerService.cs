using hotel_booking_dto;
using System.Collections.Generic;
 using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<string>> AddManagerRequest(ManagerRequestDto managerRequest);
        Task<Response<string>> SendManagerInvite(string email);
        Task<Response<bool>> CheckTokenExpiring(string email, string token);
        Task<Response<IEnumerable<ManagerRequestResponseDTo>>> GetAllManagerRequest();
    }
}
