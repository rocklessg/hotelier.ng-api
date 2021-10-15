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
            var hotelList = await _unitOfWork.Hotels.GetHotelsByRating().ToListAsync();
            var hotelListDto = _mapper.Map<IEnumerable<HotelBasicDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDto>>(StatusCodes.Status200OK, true, "hotels by ratings", hotelListDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<RoomInfoDto>>>> GetRoomByPriceAsync(PriceDto priceDto)
        {
            var roomQuery = _unitOfWork.RoomType.GetRoomByPrice(priceDto.MinPrice, priceDto.MaxPrice);
            var pageResult = await roomQuery.PaginationAsync<RoomType, RoomInfoDto>(priceDto.PageSize, priceDto.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<RoomInfoDto>>>(StatusCodes.Status200OK, true, "List of rooms by price", pageResult);
            return response;
        }

        public async Task<Response<IEnumerable<HotelBasicDto>>> GetTopDealsAsync()
        {
            var hotelList = await _unitOfWork.Hotels.GetTopDeals().ToListAsync(); ;
            var hotelListDto = _mapper.Map<IEnumerable<HotelBasicDto>>(hotelList);
            var response = new Response<IEnumerable<HotelBasicDto>>(StatusCodes.Status200OK, true, "hotels top deals", hotelListDto);
            return response;
        }

        public async Task<Response<PageResult<IEnumerable<GetAllHotelDto>>>> GetAllHotelsAsync(PagingDto paging)
        {
            var hotelQueryable = _unitOfWork.Hotels.GetAllHotels();
            var hotelList = await hotelQueryable.PaginationAsync<Hotel, GetAllHotelDto>(paging.PageSize,paging.PageNumber, _mapper);
            var response = new Response<PageResult<IEnumerable<GetAllHotelDto>>>(StatusCodes.Status200OK, true, "List of all hotels", hotelList);
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

        public async Task<Response<GetHotelDto>> GetHotelByIdAsync(string id)
        {
            var response = new Response<GetHotelDto>();
            Hotel hotel = await _unitOfWork.Hotels.GetHotelEntitiesById(id);
            if (hotel != null)
            {
                GetHotelDto hotelDto = _mapper.Map<GetHotelDto>(hotel);
                
                response.Data = hotelDto;
                response.Succeeded = true;
                response.Message = $"Details for Hotel with Id: {id}";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.Succeeded = false;
            response.Data = default;
            response.Message = $"Hotel with Id: {id} not found";
            return response;
        }

        public async Task<Response<UpdateHotelDto>> UpdateHotelAsync(string hotelId, UpdateHotelDto model)
        {
            var response = new Response<UpdateHotelDto>();
            // Get the hotel to be updated using it's Id
            Hotel hotel = await _unitOfWork.Hotels.GetHotelEntitiesById(hotelId);
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

            var checkHotelId = await _unitOfWork.Hotels.GetHotelEntitiesById(hotelId);
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
            var hotel = await _unitOfWork.Hotels.GetHotelById(hotelId);
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
        public async Task<Response<PageResult<IEnumerable<HotelBasicDto>>>> GetHotelByLocation(string location, PagingDto paging)
        {
            _logger.Information($"Attempting to get hotel in {location}");
            var hotels = _unitOfWork.Hotels.GetAllHotels()                
                .Where(q => q.State.ToLower().Contains(location.ToLower()) || q.City.ToLower().Contains(location.ToLower()));

            var response = new Response<PageResult<IEnumerable<HotelBasicDto>>>();

            if (hotels != null)
            {
                _logger.Information("Search completed successfully");
                var result = await hotels.PaginationAsync<Hotel, HotelBasicDto>
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


        public async Task<Response<PageResult<IEnumerable<ReviewToReturnDto>>>> GetAllReviewsByHotelAsync(PagingDto paging, string hotelId)
        {
            _logger.Information($"Attemp to get all review by hotel id {hotelId}");
            var response = new Response<PageResult<IEnumerable<ReviewToReturnDto>>>();
            var hotelExistCheck = await _unitOfWork.Hotels.GetHotelById(hotelId);

            if (hotelExistCheck == null)
            {
                _logger.Information("Get all reviews by hotelId failed");
                response.Succeeded = false;
                response.Data = null;
                response.Message = "Hotel does not exist";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var hotel = _unitOfWork.Hotels.GetAllReviewsByHotelAsync(hotelId);

            var pageResult = await hotel.PaginationAsync<Review, ReviewToReturnDto>(paging.PageSize, paging.PageNumber, _mapper);
            _logger.Information("Get all reviews operation successful");
            response.Succeeded = true;
            response.Data = pageResult;
            response.Message = $"List of all reviews in hotel with id {hotelId}";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

    }
}
