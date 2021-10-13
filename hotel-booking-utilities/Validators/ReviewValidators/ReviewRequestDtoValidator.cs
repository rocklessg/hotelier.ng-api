using FluentValidation;
using hotel_booking_dto.ReviewDtos;

namespace hotel_booking_utilities.Validators.ReviewValidators
{
    public class ReviewRequestDtoValidator : AbstractValidator<ReviewRequestDto>
    {
        public ReviewRequestDtoValidator()
        {
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Review can not be empty");
            RuleFor(x => x.HotelId).NotEmpty().WithMessage("Hotel ID cannot be null");
        }
    }
}
