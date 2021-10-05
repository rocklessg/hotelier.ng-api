using AutoMapper;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
