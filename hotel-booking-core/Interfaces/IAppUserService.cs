using hotel_booking_dto.AppUserDto;
using hotel_booking_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAppUserService
    {
        Task<Response<string>> UpdateAppUser(string appUserId, UpdateAppUserRequest updateAppUser);
    }
}
