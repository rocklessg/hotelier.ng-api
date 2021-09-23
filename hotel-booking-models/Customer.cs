using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class Customer
    {
        public string UsersId { get; set; }
        public string CreditCard { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public Users User { get; set; }
        public ICollection<Bookings> Booking { get; set; }
        public ICollection<WishLists> WishList { get; set; }
        public ICollection<Reviews> Review { get; set; }
    }
}
