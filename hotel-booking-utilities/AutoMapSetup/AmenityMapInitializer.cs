using AutoMapper;
using hotel_booking_dto.AmenityDtos;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.AutoMapSetup
{
    public class AmenityMapInitializer : Profile
    {
        public AmenityMapInitializer()
        {
            CreateMap<Amenity, UpdateAmenityDto>().ReverseMap();

        }
    }
}
