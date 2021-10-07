using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IAmenityRepository amenityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Response<UpdateAmenityDto> UpdateAmenity(string id, UpdateAmenityDto model)
        {
            var amenity = _unitOfWork.Amenities.GetAmenityById(id);
            var response = new Response<UpdateAmenityDto>();
            if (amenity != null)
            {
                var updatedAmenity = _mapper.Map(model, amenity);
                _unitOfWork.Amenities.UpdateAsync(updatedAmenity);
                _unitOfWork.Save();
                var result = _mapper.Map<UpdateAmenityDto>(updatedAmenity);

                response.Data = result;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Update successful";
                response.Succeeded = true;
                return response;
            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "Not found";
            response.Succeeded = false;
            return response;
        }

        public async Task<Response<AddAmenityResponseDto>> AddAmenity(string id, AddAmenityRequestDto model)
        {

            var hotel = _unitOfWork.Hotels.GetHotelByIdForAddAmenity(id);
            var response = new Response<AddAmenityResponseDto>();

            if (hotel != null)
            {
                var amenityToAdd = AddAmenityResponseDto.Add(id, model);
                await _unitOfWork.Amenities.InsertAsync(amenityToAdd);
                await _unitOfWork.Save();
                var result = _mapper.Map<AddAmenityResponseDto>(amenityToAdd);
                response.Data = result;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Amenity added successfully";
                response.Succeeded = true;
                return response;

            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = "No such hotel";
            response.Succeeded = false;
            return response;
        }

        public Response<IEnumerable<AmenityDto>> GetAmenityByHotelId(string hotelId)
        {
            var hotelAmenities = _unitOfWork.Amenities.GetAmenityByHotelId(hotelId);
            var amenityList = new List<AmenityDto>();
            foreach (var amenity in hotelAmenities)
            {
                amenityList.Add(AmenityMapper.MapToAmenityDTO(amenity));
            }
            var response = new Response<IEnumerable<AmenityDto>>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Succeeded = true,
                Data = amenityList,
                Message = $"Rooms for {hotelAmenities.Select(x => x.Hotel.Name).FirstOrDefault()}"

            };
            return response;
        }
    }
}