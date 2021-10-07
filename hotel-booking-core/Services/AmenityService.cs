using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IAmenityRepository amenityRepository,IMapper mapper, IUnitOfWork unitOfWork)
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

               // var updatedAmenity = _mapper.Map(amenity, model);
               // var result = _mapper.Map<AddAmenityResponseDto>(amenityToAdd);

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
            //var newAmenity = _mapper.Map<Amenity>(amenity);
            //await _unitOfWork.Amenities.InsertAsync(newAmenity);
            //await _unitOfWork.Save();
            //var response = _mapper.Map<AddAmenityRequestDto>(newAmenity);
            //var result = new Response<AddAmenityRequestDto>
            //{
            //    Data = response,
            //    StatusCode = StatusCodes.Status200OK,
            //    Message = "Amenity Added",
            //    Succeeded = true
            //};

            //return result;
        }
        
    }
}
