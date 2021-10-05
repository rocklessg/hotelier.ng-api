using AutoMapper;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class AmenityService
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

        public Response<UpdateAmenityDto> UpdateAmenity(string id, UpdateAmenityDto update)
        {
            var updatedAmenity = _mapper.Map<Amenity>(update);
            _amenityRepository.UpdateAsync(updatedAmenity);
            _unitOfWork.Save();
            return update;
        }
        
    }
}
