using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class ManagerRequest : BaseEntity
    {
        public string HotelName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
