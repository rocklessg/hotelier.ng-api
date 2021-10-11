using hotel_booking_dto;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelStatisticsService
    {
        Task<Response<ManagersStatisticsDto>> GetManagerStatistics(string managersId);

        Task<Response<HotelStatisticDto>> GetHotelStatistics(string hotelId);
        
        Task<Response<HotelManagerStatisticsDto>> GetHotelManagerStatistics(string managerId);




    }
}
