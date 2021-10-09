using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.CustomerDtos
{
    public class UpdateCustomerRequest
    {
        

        public string CreditCard { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        
    }
}
