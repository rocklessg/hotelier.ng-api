using FluentValidation;
using hotel_booking_dto;
using hotel_booking_dto.HotelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.HotelValidators
{
    public class UpdateHotelDtoValidator : AbstractValidator<UpdateHotelDto>
    {
        public UpdateHotelDtoValidator()
        {
            RuleFor(hotel => hotel.Name).NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name is required")
                .Matches("[A-Za-z]").WithMessage("Name can only contain alphabeths")
                .MinimumLength(2).WithMessage("Name is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.Description).NotEmpty().WithMessage("Description cannot be empty")
                .NotNull().WithMessage("Description is required")
                .Matches("[A-Za-z.,_- ]").WithMessage("Description can only contain alphabeths")
                .MinimumLength(2).WithMessage("Description is limited to a minimum of 2 characters");

            RuleFor(hotel => hotel.City).Matches("[a-zA-Z_- ]").WithMessage("City can only contain alphabeths")
                .MinimumLength(2).WithMessage("City is limited to a minimum of 2 characters");
            RuleFor(hotel => hotel.State).Matches("[a-zA-Z_- ]").WithMessage("State can only contain alphabeths")
               .MinimumLength(2).WithMessage("State is limited to a minimum of 2 characters");

        }
    }
}
