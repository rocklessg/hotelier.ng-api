using hotel_booking_dto.commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHotelService
    {
        Task<IList<HotelBasicDto>> GetHotelsByRatingsAsync(Paging paging);
    }
}
