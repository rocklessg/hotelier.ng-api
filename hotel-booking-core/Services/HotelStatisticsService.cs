using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Contexts;
using hotel_booking_data.UnitOfWork.Abstraction;
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
        private readonly HbaDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public HotelStatisticsService(HbaDbContext db, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ManagersStatisticsDto> GetManagerStatistics(string managersId)
        {
            var manager = await _unitOfWork.Managers.GetManagerStatistics(managersId);

            if (manager != null)
            {
                var managerStat = _mapper.Map<ManagersStatisticsDto>(manager);
                return managerStat;
            }

            throw new ArgumentException("This manager doesn't exist");
        }


        public async Task<int> GetTotalRoomsInEachHotel(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            

            var totalRooms = 0;


            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var rooms = _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).ToList().Count;

                totalRooms += rooms;

            }

            return totalRooms;

        }

        public async Task<int> GetTotalNoOfOccupiedRooms(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            
            var totalOccupiedRooms = 0;
            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var occupiedRooms = _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked == true).ToList().Count;

                totalOccupiedRooms += occupiedRooms;
            }
            return totalOccupiedRooms;
        }

        public decimal GetTotalEarnings(string hotelId)
        {
            var hotelBookings = _db.Bookings.Where(x => x.HotelId == hotelId).ToList();

            decimal totalPayments = 0;

            foreach (var item in hotelBookings)
            {
                var bookingId = item.Id;
                var payment = _db.Payments.Where(x => x.BookingId == bookingId).ToList();
                foreach (var x in payment)
                {
                    totalPayments += x.Amount;
                }
            }

            return totalPayments;
        }

        public async Task<int> GetTotalNoOfVacantRooms(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            var totalOccupiedRooms = 0;
            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var unoccupiedRooms = _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked != true).ToList().Count;

                totalOccupiedRooms += unoccupiedRooms;
            }

            return totalOccupiedRooms;

        }

        public async Task<int> GetNoOfRoomTypes(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            return roomType.Count;
        }

        public int GetTotalReviews(string hotelId)
        {
            var reviews = _db.Reviews.Where(x => x.HotelId == hotelId).ToList().Count;
            return reviews;
        }

        public async Task<int> GetTotalAmenities(string hotelId)
        {
            var amenities =  await _unitOfWork.Amenities.GetAmenityByHotelIdAsync(hotelId);
            return amenities.Count;
        }

        public int GetTotalBookings(string hotelId)
        {
            var totalBookings = _db.Bookings.Where(x => x.HotelId == hotelId).ToList().Count;
            return totalBookings;
        }

        public double GetAverageRatings(string hotelId)
        {
            var ratings = _db.Ratings.Where(x => x.HotelId == hotelId).ToList();
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
            var hotel = await _unitOfWork.Hotels.GetHotelsById(hotelId);
            if (hotel != null)
            {
                var totalRooms = await GetTotalRoomsInEachHotel(hotelId);
                var occupiedRooms = await GetTotalNoOfOccupiedRooms(hotelId);
                var vacantRooms = await GetTotalNoOfVacantRooms(hotelId);
                var roomTypeCount = await GetNoOfRoomTypes(hotelId);
                var reviews = GetTotalReviews(hotelId);
                var noOfAmenities = await GetTotalAmenities(hotelId);
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
            var noOfCustomers = _db.Bookings.Where(x => x.HotelId == hotelId).ToList();
            return noOfCustomers.Count;
        }

        public async Task<HotelManagerStatisticsDto> GetHotelManagerStatistics(string managerId)
        {
            var managerStats = await _unitOfWork.Managers.GetManagerStatistics(managerId);

            var averageRating = 0.0;
            var numberOfHotels = 0;
            var totalNoOfCustomers = 0;
            var occupiedRooms = 0;
            var unoccupiedRooms = 0;
            decimal transactions = 0;
            var totalRooms = 0;

            if (managerStats != null)
            {
                var hotels = _db.Hotels.Where(x => x.ManagerId == managerId).ToList();
                numberOfHotels = hotels.Count;

                foreach (var hotel in hotels)
                {
                    var hotelId = hotel.Id;
                    averageRating += GetAverageRatings(hotelId);
                    totalNoOfCustomers += GetNoOfCustomers(hotelId);
                    totalRooms += await GetTotalRoomsInEachHotel(hotelId);
                    occupiedRooms += await GetTotalNoOfOccupiedRooms(hotelId);
                    unoccupiedRooms += await GetTotalNoOfVacantRooms(hotelId);
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
