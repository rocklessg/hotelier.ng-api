using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAmenityService
    {
        IEnumerable<Amenity> GetAmenityByHotelId(string hotelId);
    }
}
