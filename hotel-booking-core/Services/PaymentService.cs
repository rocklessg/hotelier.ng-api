using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_models;
using hotel_booking_utilities.PaymentGatewaySettings;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaystackPaymentHandler _paystack;

        public PaymentService(IUnitOfWork unitOfWork, PaystackPaymentHandler paystack)
        {
            _unitOfWork = unitOfWork;
            _paystack = paystack;
        }
        public async Task<string> InitializePayment(decimal amount, Customer customer, string paymentService, string bookingId)
        {

            Payment payment = new()
            {
                BookingId = bookingId,
                Amount = amount,
                MethodOfPayment = paymentService
            };

            await _unitOfWork.Payments.InsertAsync(payment);
            await _unitOfWork.Save();

            try
            {
                if (paymentService == "PayStack")
                {
                    TransactionInitializeRequest request = new()
                    {
                        AmountInKobo = (int)(amount * 100),
                        Email = customer.AppUser.Email,
                    };
                    return _paystack.InitializePayment(request).Data.AuthorizationUrl;
                }
                throw new ArgumentException("Invalid Payment Service");
            }
            catch (ArgumentException argEx)
            {
                throw argEx;
            }
        }
    }
}
