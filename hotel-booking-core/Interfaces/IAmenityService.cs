using hotel_booking_dto;
using System.Collections.Generic;

namespace hotel_booking_core.Interfaces
{
    public interface IAmenityService
    {
        Response<IEnumerable<AmenityDto>> GetAmenityByHotelId(string hotelId);
    }
}
