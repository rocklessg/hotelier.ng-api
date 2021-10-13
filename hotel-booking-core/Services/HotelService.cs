using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.Mapper;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using hotel_booking_utilities.PaymentGatewaySettings;
using Microsoft.AspNetCore.Http;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<List<HotelBasicDto>> GetHotelsByRatingsAsync(Paging paging)
        {
            var hotelList = await _unitOfWork.Hotels.GetAllAsync(
                orderby: x => x.OrderBy(h => h.Ratings.Sum(r => r.Ratings) / h.Ratings.Count),
                Includes: new List<string>() { "Galleries" }
                );
            hotelList = hotelList.Skip(paging.PageNumber - 1).Take(paging.PageSize).ToList();
            return HotelBasicDtoMapper.MapToHotelBAsicDtoList(hotelList, _mapper);
        }

        public async Task<List<RoomInfoDto>> GetRoomByPriceAsync(PriceDto priceDto)
        {
            var roomList = await _unitOfWork.RoomType.GetAllAsync(
                Includes: new List<string>() { "Hotel" },
                expression: (roomType => (!(priceDto.MaxPrice > priceDto.MinPrice) ? roomType.Price >= priceDto.MinPrice
                                         : (roomType.Price >= priceDto.MinPrice) && (roomType.Price <= priceDto.MaxPrice)
                                         )),
                orderby: x => x.OrderBy(x => x.Price)
                );
            roomList = roomList.Skip(priceDto.PageNumber - 1).Take(priceDto.PageSize).ToList();
            return HotelBasicDtoMapper.MapToRoomInfoDtoList(roomList, _mapper);
        }

        public async Task<List<RoomInfoDto>> GetTopDealsAsync(Paging paging)
        {
            var roomList = await _unitOfWork.RoomType.GetAllAsync(
                Includes: new List<string>() { "Hotel" },
                orderby: x => x.OrderBy(rt => rt.Discount / rt.Price)
                );
            roomList = roomList.Skip(paging.PageNumber - 1).Take(paging.PageSize).ToList();
            return HotelBasicDtoMapper.MapToRoomInfoDtoList(roomList, _mapper);
        }

        public async Task<Response<List<GetHotelDto>>> GetAllHotelsAsync(Paginator paging)
        {
            List<Hotel> hotelList = (await _unitOfWork.Hotels.GetAllHotelsAsync())
                                    .Skip((paging.CurrentPage - 1) * paging.PageSize)
                                    .Take(paging.PageSize).ToList();

            var response = new Response<List<GetHotelDto>>();
            List<GetHotelDto> resultList = new();

            foreach (var hotel in hotelList)
            {
                int noOfRatings = hotel.Ratings.Count;
                double sum = hotel.Ratings.Sum(s => s.Ratings);
                double average = sum / noOfRatings;

                GetHotelDto result = new()
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Description = hotel.Description,
                    Email = hotel.Email,
                    Phone = hotel.Phone,
                    Address = hotel.Address,
                    City = hotel.City,
                    State = hotel.State,
                    Rating = average,
                    FeaturedImage = hotel.Galleries.FirstOrDefault(gallery => gallery.IsFeature == true).ImageUrl,
                    Gallery = hotel.Galleries.Select(gallery => gallery.ImageUrl),
                    RoomTypes = hotel.RoomTypes.Select(roomType =>

                        // Map Room types to Get all hotels room type DTO
                        new RoomTypeDto()
                        {
                            Id = roomType.Id,
                            Name = roomType.Name,
                            Description = roomType.Description,
                            Price = roomType.Price,
                            Discount = roomType.Discount,
                            Thumbnail = roomType.Thumbnail
                        })
                };
                resultList.Add(result);
            }
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Succeeded = true;
            response.Data = resultList;
            return response;
        }

        public Response<RoomDTo> GetHotelRooomById(string roomId)
        {
            var room = _unitOfWork.Rooms.GetHotelRoom(roomId);

            if (room != null)
            {
                var response = HotelRoomsResponse.GetResponse(room);

                var result = new Response<RoomDTo>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"is the room with id {roomId}",
                    Data = response
                };
                return result;
            }
            return Response<RoomDTo>.Fail("Not Found");
        }

        public async Task<Response<IEnumerable<RoomsByHotelDTo>>> GetAvailableRoomByHotel(Paginator paginator, string hotelId)
        {
            var roomList = await _unitOfWork.Rooms.GetAvailableRoomsByHotel(hotelId);

            if (roomList.Count() > 0)
            {
                var dtoList = HotelRoomsResponse.GetResponse(roomList);

                var item = dtoList.Skip(paginator.PageSize * (paginator.CurrentPage - 1))
                .Take(paginator.PageSize);

                var result = new Response<IEnumerable<RoomsByHotelDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = "available rooms",
                    Data = item
                };

                return result;
            }
            return Response<IEnumerable<RoomsByHotelDTo>>.Fail("Not Found");
        }


        public async Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId)
        {
            var ratings = await _unitOfWork.Hotels.HotelRatings(hotelId);

            if (ratings.Count() > 0)
            {
                var response = HotelRoomsResponse.GetResponse(ratings);

                var result = new Response<IEnumerable<HotelRatingsDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"cummulated ratings for hotel with id {hotelId}",
                    Data = response
                };

                return result;
            }
            return Response<IEnumerable<HotelRatingsDTo>>.Fail("Not found");
        }

        public Response<GetHotelDto> GetHotelById(string id)
        {
            var response = new Response<GetHotelDto>();
            Hotel hotel = _unitOfWork.Hotels.GetHotelById(id);
            if (hotel != null)
            {
                // Get the average rating for the hotel
                int numberOfRatings = hotel.Ratings.Count;
                double sumOfRatings = hotel.Ratings.Sum(s => s.Ratings);
                double average = sumOfRatings / numberOfRatings;

                // Map hotel and it's child entities to Get all hotel response DTO
                var result = new GetHotelDto()
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Description = hotel.Description,
                    Email = hotel.Email,
                    Phone = hotel.Phone,
                    Address = hotel.Address,
                    City = hotel.City,
                    State = hotel.State,
                    Rating = average,
                    FeaturedImage = hotel.Galleries.FirstOrDefault(gallery => gallery.IsFeature).ImageUrl,
                    Gallery = hotel.Galleries.Select(gallery => gallery.ImageUrl),

                    // Map Room types to Get all hotels room type DTO
                    RoomTypes = hotel.RoomTypes.Select(roomType =>
                        new RoomTypeDto()
                        {
                            Id = roomType.Id,
                            Name = roomType.Name,
                            Description = roomType.Description,
                            Price = roomType.Price,
                            Discount = roomType.Discount,
                            Thumbnail = roomType.Thumbnail
                        }),

                    // Map Reviews to Get all hotels review DTO
                    Reviews = hotel.Reviews.Select(review =>
                        new ReviewDto()
                        {
                            Text = review.Comment,
                            CustomerImage = review.Customer.AppUser.Avatar,
                            Date = review.CreatedAt.ToShortDateString()
                        }).Take(5)
                };
                response.Data = result;
                response.Succeeded = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.Data = default;
            response.Message = $"Hotel with Id = {id} not found";
            return response;

        }


        public async Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model)
        {
            var response = new Response<UpdateHotelDto>();
            // Get the hotel to be updated using it's Id
            Hotel hotel = _unitOfWork.Hotels.GetHotelById(hotelId);
            if (hotel != null)
            {
                hotel.Name = model.Name;
                hotel.Description = model.Description;
                hotel.Email = model.Email;
                hotel.Phone = model.PhoneNumber;
                hotel.Address = model.Address;
                hotel.City = model.City;
                hotel.State = model.State;
                hotel.UpdatedAt = DateTime.Now;

                // Update the hotel and save changes to database
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                // Map properties of updated hotel to the response DTO
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Succeeded = true;
                response.Message = $"Hotel with id {hotel.Id} has been updated";
                response.Data = model;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Message = $"Hotel with id {hotelId} was not found!";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<AddHotelResponseDto>> AddHotel(string managerId, AddHotelDto hotelDto)
        {
            Hotel hotel = _mapper.Map<Hotel>(hotelDto);

            hotel.ManagerId = managerId;

            await _unitOfWork.Hotels.InsertAsync(hotel);
            await _unitOfWork.Save();

            var hotelResponse = _mapper.Map<AddHotelResponseDto>(hotel);

            var response = new Response<AddHotelResponseDto>()
            {
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true,
                Data = hotelResponse,
                Message = $"{hotel.Name} with id {hotel.Id} has been added"
            };
            return response;
        }

        public async Task<Response<AddRoomResponseDto>> AddHotelRoom(string hotelId, AddRoomDto roomDto)
        {
            Room room = _mapper.Map<Room>(roomDto);

            var checkHotelId = _unitOfWork.Hotels.GetHotelById(hotelId);
            if (checkHotelId == null)
                return Response<AddRoomResponseDto>.Fail("Hotel Not Found");

            await _unitOfWork.Rooms.InsertAsync(room);
            await _unitOfWork.Save();

            var roomResponse = _mapper.Map<AddRoomResponseDto>(room);

            var response = new Response<AddRoomResponseDto>()
            {
                StatusCode = StatusCodes.Status200OK,
                Succeeded = true,
                Data = roomResponse,
                Message = $"Room with id {room.Id} added to Hotel with id {hotelId}"
            };
            return response;
        }

        public async Task<Response<HotelBookingResponseDto>> BookHotel(string hotelId, string userId, HotelBookingRequestDto bookingDto)
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
