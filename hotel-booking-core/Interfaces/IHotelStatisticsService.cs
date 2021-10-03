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
        string GetTotalHotels();
        Task<string> GetTotalRoomsInEachHotel(string hotelId);
        Task<int> GetTotalNoOfOccupiedRooms(string hotelId);

        Task<int> GetTotalNoOfVacantRooms(string hotelId);
        Task<HotelStatisticDto> GetHotelStatistics(string hotelId);
        int GetNoOfRoomTypes(string hotelId);
        int GetTotalReviews(string hotelId);
        int GetTotalAmenities(string hotelId);
        int GetTotalBookings(string hotelId);
        double GetAverageRatings(string hotelId);
        decimal GetTotalEarnings(string hotelId);




    }
}
