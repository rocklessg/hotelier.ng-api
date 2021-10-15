using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_dto.HotelDtos;
using hotel_booking_dto.ReviewDtos;
using hotel_booking_dto.RoomDtos;
using hotel_booking_models;
using hotel_booking_utilities.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        private readonly ILogger _logger;
       

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           _logger = logger;
        }

        public async Task<Response<IEnumerable<HotelAndroidDto>>> GetHotelsByRatingsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetHotelsByRating().ToListAsync();
            var hotelListDto = _mapper.Map<IEnumerable<HotelAndroidDto>>(hotelList);
            var response = new Response<IEnumerable<HotelAndroidDto>>(StatusCodes.Status200OK, true, "hotels by ratings", hotelListDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto)
        {
            var roomQuery = _unitOfWork.RoomType.GetRoomByPrice(priceDto.MinPrice, priceDto.MaxPrice);
            var pageResult = await roomQuery.PaginationAsync<RoomType, RoomInfoDto>(priceDto.PageSize, priceDto.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<RoomInfoDto>>>(StatusCodes.Status200OK, true, "List of rooms by price", pageResult);
            return response;
        }

        public async Task<Response<IEnumerable<HotelAndroidDto>>> GetTopDealsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetTopDeals().ToListAsync(); ;
            var hotelListDto = _mapper.Map<IEnumerable<HotelAndroidDto>>(hotelList);
            var response = new Response<IEnumerable<HotelAndroidDto>>(StatusCodes.Status200OK, true, "hotels top deals", hotelListDto);
            return response;
        }

        public async Task<Response<List<GetHotelDto>>> GetAllHotelsAsync(PagingDto paging)
        {
            List<Hotel> hotelList = (await _unitOfWork.Hotels.GetAllHotelsAsync())
                                    .Skip((paging.PageNumber - 1) * paging.PageSize)
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

        public async Task<Response<IEnumerable<RoomDTo>>> GetHotelRooomById(string hotelId, string roomTypeId)
        {
            var room = await _unitOfWork.Rooms.GetHotelRoom(hotelId, roomTypeId);

            if (room != null)
            {
                var response = _mapper.Map<IEnumerable<RoomDTo>>(room);

                var result = new Response<IEnumerable<RoomDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"Hotel Rooms for roomType with id {roomTypeId} in hotel with  id {hotelId}",
                    Data = response
                };
                return result;
            }
            return Response<IEnumerable<RoomDTo>>.Fail("No room found for this particular roomtype", StatusCodes.Status404NotFound);
        }

        public async Task<Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>> GetHotelRoomType(PagingDto paging, string hotelId)
        {
            var roomList = _unitOfWork.Rooms.GetRoomTypeByHotel(hotelId);

            if (roomList.Any())
            {
                var item = await roomList.PaginationAsync<RoomType, RoomTypeByHotelDTo>(paging.PageSize, paging.PageNumber, _mapper);
                var result = new Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"Total RoomType in hotel with id {hotelId}",
                    Data = item
                };
                return result;
            }
            return Response<PageResult<IEnumerable<RoomTypeByHotelDTo>>>.Fail("Hotel is not valid", StatusCodes.Status404NotFound);
        }

        public async Task<Response<IEnumerable<HotelRatingsDTo>>> GetHotelRatings(string hotelId)
        {
            var ratings = await _unitOfWork.Hotels.HotelRatings(hotelId);

            if (ratings.Any())
            {
                var response = _mapper.Map<IEnumerable<HotelRatingsDTo>>(ratings);

                var result = new Response<IEnumerable<HotelRatingsDTo>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Succeeded = true,
                    Message = $"cummulated ratings for hotel with id {hotelId}",
                    Data = response
                };

                return result;
            }
            return Response<IEnumerable<HotelRatingsDTo>>.Fail("No ratings for this hotel", StatusCodes.Status404NotFound);
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

                    // Map Room types to Hotel room type DTO
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

                    // Map Amenities to Hotel Amenities DTO
                    Amenities = hotel.Amenities.Select(amenity =>
                        new AmenityDto()
                        {
                            Id = amenity.Id,
                            Name = amenity.Name,
                            Price = amenity.Price,
                            Discount = amenity.Discount,
                        }),

                    // Map Reviews to Hotel review DTO
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

        public async Task<Response<string>> DeleteHotelByIdAsync(string hotelId)
        {
            var hotel = _unitOfWork.Hotels.GetHotelById(hotelId);
            var response = new Response<string>();

            if (hotel != null)
            {
                _unitOfWork.Hotels.DeleteAsync(hotel);
                await _unitOfWork.Save();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = $"Hotel with Id = {hotelId} has been deleted";
                response.Data = default;
                response.Succeeded = true;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = $"Hotel with id = {hotelId} does not exist";
            response.Succeeded = false;
            return response;
        }
        public async Task<Response<PageResult<IEnumerable<HotelAndroidDto>>>> GetHotelByLocation(string location, PagingDto paging)
        {
            _logger.Information($"Attempting to get hotel in {location}");
            var hotels = _unitOfWork.Hotels.GetAllHotels()                
                .Where(q => q.State.ToLower().Contains(location.ToLower()) || q.City.ToLower().Contains(location.ToLower()));

            var response = new Response<PageResult<IEnumerable<HotelAndroidDto>>>();

            if (hotels != null)
            {
                _logger.Information("Search completed successfully");
                var result = await hotels.PaginationAsync<Hotel, HotelAndroidDto>
                    (
                        pageSize: paging.PageSize, 
                        pageNumber: paging.PageNumber, 
                        mapper: _mapper
                    );

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
