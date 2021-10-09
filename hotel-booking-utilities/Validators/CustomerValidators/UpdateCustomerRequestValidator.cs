using FluentValidation;
using hotel_booking_dto.CustomerDtos;
using hotel_booking_utilities.ValidatorSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Validators.CustomerValidators
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(user => user.CreditCard).CreditCard();
            RuleFor(user => user.Address).Address();
            RuleFor(user => user.State).State();
            

        }
    }
}
