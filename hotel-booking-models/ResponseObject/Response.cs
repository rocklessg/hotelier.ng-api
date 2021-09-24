using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models.ResponseObject
{
    public class Response<T>
    {
        public bool Success { get; set; } = false;
        public T Data { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
