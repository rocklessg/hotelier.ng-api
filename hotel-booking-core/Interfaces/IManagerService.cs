﻿using hotel_booking_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IManagerService
    {
        Task<Response<string>> SoftDeleteManagerAsync(string managerId);
    }
}