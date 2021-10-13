using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;

namespace hotel_booking_core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<TransactionResponseDto>>> GetManagerTransactionsAsync(string managerId, /*string filter, string searchQuery, */Paginator paging)
        {
            var response = new Response<List<TransactionResponseDto>>();
            List<TransactionResponseDto> transactionList = new();

            var manager = await _unitOfWork.Managers.GetManagerAsync(managerId);
            if(manager != null)
            {
                var bookings = manager.Hotels.Select(h => h.Bookings)
               .Skip((paging.CurrentPage - 1) * paging.PageSize)
               .Take(paging.PageSize).ToList();
                    foreach (Booking booking in bookings)
                    {
                        TransactionResponseDto transaction = new()
                        {
                            BookingId = booking.Id,
                            BookingReference = booking.BookingReference,
                            CheckIn = booking.CheckIn,
                            CheckOut = booking.CheckOut,
                            NoOfPeople = booking.NoOfPeople,
                            ServiceName = booking.ServiceName,
                            HotelId = booking.HotelId,
                            HotelName = booking.Hotel.Name,
                            CustomerId = booking.CustomerId,
                            CustomeName = $"{booking.Customer.AppUser.FirstName} {booking.Customer.AppUser.LastName}",
                            PaymentId = booking.Payment.BookingId,
                            PaymentAmount = booking.Payment.Amount,
                            PaymentDate = booking.Payment.CreatedAt
                        };
                        transactionList.Add(transaction);
                    };
                    
                response.Data = null;
                response.Succeeded = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.Data = default;
            response.Message = $"Manager with Id = {managerId} not found";
            return response;
        }
    }
}
