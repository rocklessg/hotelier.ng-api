using hotel_booking_dto.CustomerDtos;
using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.Mapper
{
    public class GetUsersResponseDtoMap
    {
        public static GetUsersResponseDto GetUsersResponse(Customer customer)
        {
            return new GetUsersResponseDto()
            {
                FirstName = customer.AppUser.FirstName,
                LastName = customer.AppUser.LastName,
                Age = customer.AppUser.Age,
                Id = customer.AppUser.Id,
                Email = customer.AppUser.Email,
                PhoneNumber = customer.AppUser.PhoneNumber,
                UserName = customer.AppUser.UserName,
                CreatedAt = customer.AppUser.CreatedAt
            };
        }
    }
}
