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
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
       

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           _logger = logger;
        }

        public async Task<Response<IEnumerable<HotelBasicDto>>> GetHotelsByRatingsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetHotelsByRatingAsync();
            var hotelDto = _mapper.Map<IEnumerable<HotelBasicDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDto>>(StatusCodes.Status200OK, true, "Top 5 hotels by ratings", hotelDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto)
        {
            var roomQuery = _unitOfWork.RoomType.GetRoomByPrice(priceDto.MinPrice, priceDto.MaxPrice);
            var pageResult = await roomQuery.PaginationAsync<RoomType, RoomInfoDto>(priceDto.PageSize, priceDto.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<RoomInfoDto>>>(StatusCodes.Status200OK, true, "List of rooms by price", pageResult);
            return response;
        }

        public async Task<Response<IEnumerable<RoomInfoDto>>> GetTopDealsAsync()
        {
            var roomList = await _unitOfWork.RoomType.GetTopDealsAsync();
            var roomDto = _mapper.Map<IEnumerable<RoomInfoDto>>(roomList);
            var response = new Response<IEnumerable<RoomInfoDto>>(StatusCodes.Status200OK, true, "Top 5 Deals", roomDto);
            return response;
        }

        public async Task<Response<List<GetHotelDto>>> GetAllHotelsAsync(Paginator paging)
        {
            List<Hotel> hotelList = (await _unitOfWork.Hotels.GetAllHotelsAsync())
                                    .Skip((paging.CurrentPage - 1)*paging.PageSize)
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
            if(hotel!=null)
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

        public async Task<Response<PageResult<IEnumerable<HotelBasicDto>>>> GetHotelByLocation(string location, Paging paging)
        {
            _logger.Information($"Attempting to get hotel in {location}");
            var hotels = _unitOfWork.Hotels.GetAllHotels()
                .Where(q => q.State.Contains(location) || q.City.Contains(location));

            var response = new Response<PageResult<IEnumerable<HotelBasicDto>>>();

            if (hotels != null)
            {
                _logger.Information("Search completed successfully");
                var result = await hotels.PaginationAsync<Hotel, HotelBasicDto>(pageSize: paging.PageSize, pageNumber: paging.PageNumber, mapper: _mapper);

                response.Data = result;
                response.StatusCode = StatusCodes.Status200OK;
                response.Succeeded = true;
                return response;
            }

            _logger.Information("Search completed with no results");
            response.Data = default;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "On your request nothing has been found.";
            response.Succeeded = false;
            return response;
        }

    }
}
