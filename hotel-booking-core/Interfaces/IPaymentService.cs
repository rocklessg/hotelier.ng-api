using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IPaymentService
    {
        Task<string> InitializePayment(decimal amount, Customer customer, string paymentService, string bookingId);
    }
}
