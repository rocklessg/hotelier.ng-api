using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class Payment : BaseModel
    {
        public string BookingId { get; set; }
        public string TransactionReference { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string MethodOfPayment { get; set; }
        public Bookings Booking { get; set; }
        
    }
}
