using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IBookingService
    {
        Task<Response<IEnumerable<Booking>>> GetCustomerBookings(string userId, Paginator paginator);
        Task<Response<HotelBookingResponseDto>> Book(string hotelId, string userId, HotelBookingRequestDto bookingDto);
    }
}
