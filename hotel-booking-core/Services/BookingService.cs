using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<Response<IEnumerable<Booking>>> GetCustomerBookings(string userId, Paginator paginator)
        {
            Customer customer = await _unitOfWork.Customers.GetCustomerAsync(userId);
            if (customer == null)
            {
                return Response<IEnumerable<Booking>>.Fail("No customer found");
            }
            var bookings = customer.Bookings.Skip((paginator.CurrentPage - 1) * paginator.PageSize).Take(paginator.PageSize).ToArray();
            Response<IEnumerable<Booking>> response = new() 
            { 
                Data = bookings, 
                Message = "Customer Bookings Fetched", 
                StatusCode = StatusCodes.Status200OK, 
                Succeeded = true 
            };
            return response;
        }

        public async Task<Response<HotelBookingResponseDto>> Book(string hotelId, string userId, HotelBookingRequestDto bookingDto)
        {
            Hotel hotel = _unitOfWork.Hotels.GetHotelById(hotelId);

            if (hotel == null)
            {
                return Response<HotelBookingResponseDto>.Fail("Hotel not Found");
            }

            Room room = _unitOfWork.Rooms.GetHotelRoom(bookingDto.RoomId);
            if (room == null)
            {
                return Response<HotelBookingResponseDto>.Fail("Room not found");
            }

            if (room.Roomtype.HotelId != hotelId)
            {
                return Response<HotelBookingResponseDto>.Fail("Room not found in selected Hotel");
            }

            if (room.IsBooked)
            {
                return Response<HotelBookingResponseDto>.Fail("Room already booked", StatusCodes.Status422UnprocessableEntity);
            }

            Customer customer = await _unitOfWork.Customers.GetCustomerAsync(userId);

            Booking booking = _mapper.Map<Booking>(bookingDto);
            booking.CustomerId = customer.AppUserId;
            booking.BookingReference = $"HBA-${DateTime.Today.Millisecond}";
            booking.HotelId = hotelId;

            room.IsBooked = true;

            await _unitOfWork.Booking.InsertAsync(booking);
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.Save();

            decimal amount = room.Roomtype.Price;

            try
            {
                string authorizationUrl = await _paymentService.InitializePayment(amount, customer, bookingDto.PaymentService, booking.Id);
                HotelBookingResponseDto bookingResponse = _mapper.Map<HotelBookingResponseDto>(booking);
                bookingResponse.PaymentUrl = authorizationUrl;
                Response<HotelBookingResponseDto> response = new Response<HotelBookingResponseDto>()
                {
                    Data = bookingResponse,
                    Message = "Booking Reserved",
                    StatusCode = StatusCodes.Status201Created,
                    Succeeded = true
                };
                return response;
            }
            catch (ArgumentException argEx)
            {
                return Response<HotelBookingResponseDto>.Fail(argEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return Response<HotelBookingResponseDto>.Fail(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
