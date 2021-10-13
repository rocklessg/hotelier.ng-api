using Microsoft.Extensions.Configuration;
using PayStack.Net;
using System;

namespace hotel_booking_utilities.PaymentGatewaySettings
{
    public class PaystackPaymentHandler
    {
        private readonly IConfiguration _configuration;

        public PayStackApi PayStack { get; set; }
        public PaystackPaymentHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TransactionInitializeResponse InitializePayment(TransactionInitializeRequest request)
        {
            var secret = _configuration["Payment:PaystackKey"];
            PayStack = new PayStackApi(secret);
            var response = PayStack.Transactions.Initialize(request);
            if (response.Status)
            {
                return response;
            }
            throw new ArgumentException(response.Message);
        }
    }
}
