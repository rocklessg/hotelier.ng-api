<<<<<<< HEAD
﻿using hotel_booking_dto.AppUserDto;
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
=======
﻿using hotel_booking_dto;
using hotel_booking_dto.CustomerDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public interface IAppUserService
    {
        Task<Response<UpdateUserImageDto>> UpdateCustomerPhoto(string customerId, string url);
    }
}
>>>>>>> 29f8332fa1369f86dff22eccae8cf40bacbfae10
