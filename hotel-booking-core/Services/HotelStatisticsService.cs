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
using System.Net;
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

        public async Task<Response<ManagersStatisticsDto>> GetManagerStatistics(string managersId)
        {
            var manager = await _unitOfWork.Managers.GetManagerStatistics(managersId);
            var response = new Response<ManagersStatisticsDto>();

            if (manager != null)
            {
                
                var managerStat = _mapper.Map<ManagersStatisticsDto>(manager);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = managerStat;
                response.Message = $"are the statistics for manager with {managerStat.AppUserId}";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = true;
            response.Message = $"Manager with Id = { managersId} doesn't exist";
            return response;
        }


        public async Task<int> GetTotalRoomsInEachHotel(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            

            var totalRooms = 0;


            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var roomList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).ToListAsync();
                var rooms = roomList.Count;

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
                var occupiedRoomsList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked == true).ToListAsync();
                var occupiedRooms = occupiedRoomsList.Count;

                totalOccupiedRooms += occupiedRooms;
            }
            return totalOccupiedRooms;
        }

        public async Task<decimal> GetTotalEarnings(string hotelId)
        {
            var hotelBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();

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

        public async Task<decimal> GetMonthlyEarnings(string hotelId) 
        {
            string sMonth = "06";
            var hotelBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();

            decimal totalMonthlyPayments = 0;

            foreach (var item in hotelBookings)
            {
                var bookingId = item.Id;
                var payment = await _db.Payments.Where(x => x.BookingId == bookingId).ToListAsync();

                foreach(var x in payment) 
                {
                    var paymentTime = x.CreatedAt.ToString();
                    var paymentMonth = paymentTime.Substring(3, 2);
                    if(sMonth == paymentMonth) 
                    {
                        totalMonthlyPayments += x.Amount;
                    }
                }
            }

            return totalMonthlyPayments;
        }

        public async Task<int> GetTotalNoOfVacantRooms(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            var totalOccupiedRooms = 0;
            foreach (var item in roomType)
            {
                var roomTypeId = item.Id;
                var unoccupiedRoomsList = await _db.Rooms.Where(x => x.RoomTypeId == roomTypeId).
                                 Where(z => z.IsBooked != true).ToListAsync();

                var unoccupiedRooms = unoccupiedRoomsList.Count;
                totalOccupiedRooms += unoccupiedRooms;
            }

            return totalOccupiedRooms;

        }

        public async Task<int> GetNoOfRoomTypes(string hotelId)
        {
            var roomType = await _unitOfWork.RoomType.GetRoomTypesInEachHotel(hotelId);
            return roomType.Count;
        }

        public async Task<int> GetTotalReviews(string hotelId)
        {
            var reviews = await _db.Reviews.Where(x => x.HotelId == hotelId).ToListAsync();
            return reviews.Count;
        }

        public async Task<int> GetTotalAmenities(string hotelId)
        {
            var amenities =  await _unitOfWork.Amenities.GetAmenityByHotelIdAsync(hotelId);
            return amenities.Count;
        }

        public async Task<int> GetTotalBookings(string hotelId)
        {
            var totalBookings = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();
            return totalBookings.Count;
        }

        public async Task<double> GetAverageRatings(string hotelId)
        {
            var ratings = await _db.Ratings.Where(x => x.HotelId == hotelId).ToListAsync();
            double totalRatings = 0;
            foreach (var item in ratings)
            {
                totalRatings += Convert.ToDouble(item.Ratings);
            }

            var averageRatings = totalRatings / ratings.Count;
            return averageRatings;
        }



        public async Task<Response<HotelStatisticDto>> GetHotelStatistics(string hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelsById(hotelId);
            var response = new Response<HotelStatisticDto>();
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

                var hotelstatistics = new HotelStatisticDto
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
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = hotelstatistics;
                response.Message = $"are the statistics for hotel with {hotel.Id}";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = true;
            response.Message = $"Hotel with Id = { hotel.Id} doesn't exist";
            return response;
        }

        public async Task<int> GetNoOfCustomers(string hotelId)
        {
            var noOfCustomers = await _db.Bookings.Where(x => x.HotelId == hotelId).ToListAsync();
            return noOfCustomers.Count;
        }

        public async Task<Response<HotelManagerStatisticsDto>> GetHotelManagerStatistics(string managerId)
        {
            var managerStats = await _unitOfWork.Managers.GetManagerStatistics(managerId);
            var response = new Response<HotelManagerStatisticsDto>();

            var averageRating = 0.0;
            var numberOfHotels = 0;
            var totalNoOfCustomers = 0;
            var availableRooms = 0;
            var bookedRooms = 0;
            decimal monthlyTransactions = 0;
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
                    availableRooms += await GetTotalNoOfOccupiedRooms(hotelId);
                    bookedRooms += await GetTotalNoOfVacantRooms(hotelId);
                    monthlyTransactions += GetMonthlyEarnings(hotelId);

                }

                var hotelManagerStats = new HotelManagerStatisticsDto
                {
                    TotalHotels = numberOfHotels,
                    AverageHotelRatings = averageRating,
                    TotalNumberOfCustomers = totalNoOfCustomers,
                    TotalRooms = totalRooms,
                    BookedRooms = bookedRooms,
                    AvailableRooms = availableRooms,
                    TotalMonthlyTransactions = monthlyTransactions

    };
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Data = hotelManagerStats;
                response.Message = $"are the statistics for the hotel manager with {managerId}";
                return response;
            }

            response.Data = default;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Succeeded = true;
            response.Message = $"No record exists for this manager";
            return response;

        }



    }
}
