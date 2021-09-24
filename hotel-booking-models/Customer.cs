using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models
{
    public class Customer : BaseEntity
    {
        public string CreditCard { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public AppUser User { get; set; }
        public ICollection<Booking> Booking { get; set; }
        public ICollection<WishList> WishList { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Rating> Rating { get; set; }
    }
}
