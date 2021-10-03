using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelStatisticsService : IHotelStatisticsService
    {
        private readonly HbaDbContext db;

        public HotelStatisticsService(HbaDbContext db)
        {
            this.db = db;
        }
        
        public string GetTotalHotels() 
        {
            var hotelCount = db.Hotels.Count();
            return $"Total hotels : {hotelCount}";
        }

        public async Task<string> GetTotalRoomsInEachHotel(string hotelId) 
        {
            var hotel = await db.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();
          
            int totalRooms = 0;
            if(hotel != null) 
            {
                var roomType = db.RoomTypes.Where(x => x.HotelId == hotelId).ToList();
                foreach(var item in roomType)
                {
                    var roomTypeId = item.Id;
                    var rooms = db.Rooms.Where(x => x.RoomTypeId == roomTypeId).ToList().Count;

                    totalRooms += rooms;

                }

                return $"Total No. of Rooms: {totalRooms}";
            }

            throw new ArgumentNullException("The hotel doesn't exist");
        }

        public async Task<int> GetTotalNoOfOccupiedRooms(string hotelId) 
        {
            var hotel = await db.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();
            if(hotel != null) 
            {
                var roomType = db.RoomTypes.Where(x => x.HotelId == hotelId).ToList();
                var totalOccupiedRooms = 0;
                foreach (var item in roomType)
                {
                    var roomTypeId = item.Id;
                    var occupiedRooms = db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked == true).ToList().Count;

                    totalOccupiedRooms += occupiedRooms;
                }

                return totalOccupiedRooms;
            }

            throw new ArgumentNullException("The hotel doesn't exist");

        }

        public decimal GetTotalEarnings(string hotelId)
        {
            var hotelBookings = db.Bookings.Where(x => x.HotelId == hotelId).ToList();
            decimal totalPayments = 0;

            foreach (var item in hotelBookings)
            {
                var bookingId = item.Id;
                var payment = db.Payments.Where(x => x.BookingId == bookingId).ToList();
                foreach(var x in payment) 
                {
                    totalPayments += x.Amount;
                }
            }

            return totalPayments;
        }

        public async Task<int> GetTotalNoOfVacantRooms(string hotelId)
        {
            var hotel = await db.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();

            if (hotel != null) 
            {
                var roomType = db.RoomTypes.Where(x => x.HotelId == hotelId).ToList();
                var totalOccupiedRooms = 0;
                foreach (var item in roomType)
                {
                    var roomTypeId = item.Id;
                    var unoccupiedRooms = db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked != true).ToList().Count;

                    totalOccupiedRooms += unoccupiedRooms;
                }

                return totalOccupiedRooms;
            }

            throw new ArgumentNullException("This hotel doesn't exist");
        }

        public int GetNoOfRoomTypes(string hotelId) 
        {
            var roomType = db.RoomTypes.Where(x => x.HotelId == hotelId).ToList().Count;
            return roomType;
        }

        public int GetTotalReviews(string hotelId) 
        {
            var reviews = db.Reviews.Where(x => x.HotelId == hotelId).ToList().Count;
            return reviews;
        }

        public int GetTotalAmenities(string hotelId)
        {
            var amenitiesCount = db.Amenities.Where(x => x.HotelId == hotelId).ToList().Count;
            return amenitiesCount;
        }

        public int GetTotalBookings(string hotelId) 
        {
            var totalBookings = db.Bookings.Where(x => x.HotelId == hotelId).ToList().Count;
            return totalBookings;
        }

        public double GetAverageRatings(string hotelId) 
        {
            var ratings = db.Ratings.Where(x => x.HotelId == hotelId).ToList();
            double totalRatings = 0;
            foreach (var item in ratings)
            {
                totalRatings += Convert.ToDouble(item.Ratings);
            }

            var averageRatings = totalRatings / ratings.Count;
            return averageRatings;
        }

       

        public async Task<HotelStatisticDto> GetHotelStatistics(string hotelId) 
        {
            var hotel = await db.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();
            if(hotel != null) 
            {
                var totalRooms = await GetTotalRoomsInEachHotel(hotelId);
                var occupiedRooms = await GetTotalNoOfOccupiedRooms(hotelId);
                var vacantRooms = await GetTotalNoOfVacantRooms (hotelId);
                var roomTypeCount = GetNoOfRoomTypes(hotelId);
                var reviews = GetTotalReviews(hotelId).ToString();
                var noOfAmenities = GetTotalAmenities(hotelId).ToString();
                var totalBookings = GetTotalBookings(hotelId).ToString();
                var averageRatings = GetAverageRatings(hotelId).ToString();
                var totalEarnings = GetTotalEarnings(hotelId).ToString();

                HotelStatisticDto hotelstatistics = new HotelStatisticDto
                {
                    Name = hotel.Name,
                    NumberOfRooms = totalRooms.ToString(),
                    AverageRatings = averageRatings,
                    RoomsOccupied = occupiedRooms.ToString(),
                    RoomsUnOccupied = vacantRooms.ToString(),
                    NumberOfReviews = reviews,
                    TotalNumberOfBookings = totalBookings.ToString(),
                    TotalEarnings = totalEarnings,
                    RoomTypes = roomTypeCount.ToString(),
                    NumberOfAmenities = noOfAmenities.ToString()
                };
                return hotelstatistics;
            }

            throw new ArgumentNullException("This hotel doesn't exist");
        }


       

    }
}
