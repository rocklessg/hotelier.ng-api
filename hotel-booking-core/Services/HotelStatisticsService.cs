using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<HotelStatisticsService> _logger;

        public HotelStatisticsService(HbaDbContext db, ILogger<HotelStatisticsService> logger)
        {
            this.db = db;
            _logger = logger;
        }
        
        public int GetTotalHotels() 
        {
            var hotelCount = db.Hotels.Count();
            return hotelCount;
        }

        public int GetTotalRoomsInEachHotel(string hotelId) 
        {
            var roomType = db.RoomTypes.Where(x => x.HotelId == hotelId).ToList();
            var totalRooms = 0;


            foreach (var item in roomType)
                {
                    var roomTypeId = item.Id;
                    var rooms = db.Rooms.Where(x => x.RoomTypeId == roomTypeId).ToList().Count;

                    totalRooms += rooms;

                }

                return totalRooms;
           
        }

        public int GetTotalNoOfOccupiedRooms(string hotelId) 
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

        public int GetTotalNoOfVacantRooms(string hotelId)
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
                var totalRooms = GetTotalRoomsInEachHotel(hotelId);
                var occupiedRooms = GetTotalNoOfOccupiedRooms(hotelId);
                var vacantRooms = GetTotalNoOfVacantRooms (hotelId);
                var roomTypeCount = GetNoOfRoomTypes(hotelId);
                var reviews = GetTotalReviews(hotelId);
                var noOfAmenities = GetTotalAmenities(hotelId);
                var totalBookings = GetTotalBookings(hotelId);
                var averageRatings = GetAverageRatings(hotelId);
                var totalEarnings = GetTotalEarnings(hotelId);

                HotelStatisticDto hotelstatistics = new HotelStatisticDto
                {
                    Name = hotel.Name,
                    NumberOfRooms = totalRooms,
                    AverageRatings = averageRatings,
                    RoomsOccupied = occupiedRooms,
                    RoomsUnOccupied = vacantRooms,
                    NumberOfReviews = reviews,
                    TotalNumberOfBookings = totalBookings,
                    TotalEarnings = totalEarnings,
                    RoomTypes = roomTypeCount,
                    NumberOfAmenities = noOfAmenities
                };
                return hotelstatistics;
            }

            throw new ArgumentNullException("This hotel doesn't exist");
        }

        public int GetNoOfCustomers(string hotelId) 
        {
            var noOfCustomers = db.Bookings.Where(x => x.HotelId == hotelId).ToList();
            return noOfCustomers.Count;
        }

        public async Task<HotelManagerStatisticsDto> GetHotelManagerStatistics(string managerId)
        {
            var manager = await db.Managers.Where(x => x.AppUserId == managerId).FirstOrDefaultAsync();
            var averageRating = 0.0;
            var numberOfHotels = 0;
            var totalNoOfCustomers = 0;
            var occupiedRooms = 0;
            var unoccupiedRooms = 0;
            decimal transactions = 0;
            var totalRooms = 0;
            
            if(manager != null) 
            {
                var hotels = db.Hotels.Where(x => x.ManagerId == managerId).ToList();
                numberOfHotels = hotels.Count;

                foreach (var hotel in hotels) 
                {
                    var hotelId = hotel.Id;
                    averageRating += GetAverageRatings(hotelId);
                    totalNoOfCustomers += GetNoOfCustomers(hotelId);
                    totalRooms += GetTotalRoomsInEachHotel(hotelId);
                    occupiedRooms += GetTotalNoOfOccupiedRooms(hotelId);
                    unoccupiedRooms += GetTotalNoOfVacantRooms(hotelId);
                    transactions += GetTotalEarnings(hotelId);

                }

                var hotelManagerStats = new HotelManagerStatisticsDto
                {
                    NumberOfHotels = numberOfHotels,
                    AverageHotelRatings = averageRating,
                    TotalNumberOfCustomers = totalNoOfCustomers,
                    TotalManagerRooms = totalRooms,
                    TotalManagerOccupiedRooms = occupiedRooms,
                    TotalManagerUnoccupiedRooms = unoccupiedRooms,
                    TotalManagerTransactionAmount = transactions
                };

                return hotelManagerStats;
            }

            throw new ArgumentNullException("No record exists for this manager");

        }



    }
}
