using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto.PaymentDtos;
using hotel_booking_models;
using hotel_booking_utilities.Exceptions;
using hotel_booking_utilities.PaymentGatewaySettings;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaystackPaymentHandler _paystack;
        private readonly FlutterwavePaymentHandler _flutterwave;

        public PaymentService(IUnitOfWork unitOfWork, PaystackPaymentHandler paystack, FlutterwavePaymentHandler flutterwave)
        {
            _unitOfWork = unitOfWork;
            _paystack = paystack;
            _flutterwave = flutterwave;
        }
        public async Task<string> InitializePayment(decimal amount, Customer customer, string paymentService, string bookingId, string transactionRef, string redirect_url)
        {

            Payment payment = new()
            {
                BookingId = bookingId,
                Amount = amount,
                MethodOfPayment = paymentService,
                TransactionReference = transactionRef,
            };

            await _unitOfWork.Payments.InsertAsync(payment);
            await _unitOfWork.Save();

            try
            {
                if (paymentService.ToLower() == "paystack")
                {
                    TransactionInitializeRequest request = new()
                    {
                        AmountInKobo = (int)(amount * 100),
                        Email = customer.AppUser.Email,
                        Reference = transactionRef,
                        CallbackUrl = redirect_url
                    };
                    return _paystack.InitializePayment(request).Data.AuthorizationUrl;
                } else if(paymentService.ToLower() == "flutterwave")
                {
                    FlutterwaveRequestDTO request = new()
                    {
                        amount = amount,
                        tx_ref = transactionRef,
                        redirect_url = redirect_url,
                        payment_options = new List<string>() { "card", "mobilemoney", "ussd"},
                        customer = new() { email = customer.AppUser.Email, name = $"{customer.AppUser.FirstName} {customer.AppUser.LastName}"}
                    };
                    var response = await _flutterwave.InitializePayment(request);
                    return response.Data.Link;
                }
                throw new PaymentException("Invalid Payment Service");
            }
            catch (PaymentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> VerifyTransaction(string transactionRef, string paymentMethod, string transactionId = null)
        {
            if(paymentMethod.ToLower() == "paystack")
            {
                return _paystack.VerifyTransaction(transactionRef).Status;
            }
            else if (paymentMethod.ToLower() == "flutterwave")
            {
                var response = await _flutterwave.VerifyTransaction(transactionId);
                if(response.Data.Status == "successful")
                {
                    return true;
                }
                throw new PaymentException(response.Message);
            }
            throw new ArgumentException("Invalid Payment Method");
        }
    }
}
