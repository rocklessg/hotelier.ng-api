using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.BookingDtos;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, IConfiguration configuration, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Response<PageResult<IEnumerable<GetBookingResponseDto>>>> GetCustomerBookings(string userId, PagingDto paging)
        {
            _logger.Information($"Attempt to get customer bookings for customer with Id {userId}");
            var customer = await _unitOfWork.Customers.GetCustomerAsync(userId);
            if (customer == null)
            {
                _logger.Error($"customer with Id {userId} not found");
                return Response<PageResult<IEnumerable<GetBookingResponseDto>>>.Fail("Customer not found");
            }
            var bookings = _unitOfWork.Booking.GetBookingsByCustomerId(userId);
            var pageResult = await bookings.PaginationAsync<Booking, GetBookingResponseDto>(paging.PageSize, paging.PageNumber, _mapper);
            Response<PageResult<IEnumerable<GetBookingResponseDto>>> response = new()
            {
                Data = pageResult,
                Message = "Customer Bookings Fetched",
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true
            };
            _logger.Information($"Get customer bookings for customer with Id {userId} succesful");
            return response;
        }

        public async Task<Response<HotelBookingResponseDto>> Book(string userId, HotelBookingRequestDto bookingDto)
        {
            _logger.Information($"Attempt to book hotel for customer with Id {userId}");
            var room = _unitOfWork.Rooms.GetRoomById(bookingDto.RoomId);
            if (room == null)
            {
                _logger.Error($"Room with Id {bookingDto.RoomId} not found");
                return Response<HotelBookingResponseDto>.Fail("Room not found");
            }

            if (room.IsBooked)
            {
                _logger.Error($"Room with Id {bookingDto.RoomId} already booked");
                return Response<HotelBookingResponseDto>.Fail("Room already booked", StatusCodes.Status422UnprocessableEntity);
            }

            Customer customer = await _unitOfWork.Customers.GetCustomerAsync(userId);

            Booking booking = _mapper.Map<Booking>(bookingDto);
            booking.CustomerId = customer.AppUserId;
            booking.BookingReference = $"HBA-{ReferenceGen.GetInitials(room.Roomtype.Hotel.Name)}-{ReferenceGen.Generate()}";
            booking.HotelId = room.Roomtype.HotelId;

            await _unitOfWork.Booking.InsertAsync(booking);

            decimal amount = room.Roomtype.Price - (room.Roomtype.Price * room.Roomtype.Discount);

            string transactionRef = $"{ReferenceGen.Generate()}";

            string authorizationUrl = await _paymentService.InitializePayment(amount, customer, bookingDto.PaymentService, booking.Id, transactionRef, _configuration["Payment:RedirectUrl"]);
            HotelBookingResponseDto bookingResponse = _mapper.Map<HotelBookingResponseDto>(booking);
            bookingResponse.PaymentUrl = authorizationUrl;
            Response<HotelBookingResponseDto> response = new Response<HotelBookingResponseDto>()
            {
                Data = bookingResponse,
                Message = "Booking Reserved",
                StatusCode = StatusCodes.Status201Created,
                Succeeded = true
            };

            room.IsBooked = true;
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.Save();
            _logger.Information($"Room with Id {bookingDto.RoomId} booked successfully");
            return response;
        }
    }
}
