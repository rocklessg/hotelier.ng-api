using FluentValidation;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_utilities.ValidatorSettings;

namespace hotel_booking_utilities.Validators.CustomerValidators
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(user => user.CreditCard).CreditCard();
            RuleFor(user => user.Address).Address();
            RuleFor(user => user.State).State();           

        }
    }
}
