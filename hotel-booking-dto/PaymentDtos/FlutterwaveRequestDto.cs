using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.PaymentDtos
{
    public class FlutterwaveRequestDTO
    {
        [Required]
        public decimal amount { get; set; }
        [Required]
        public string tx_ref { get; set; }
        [Required]
        public string redirect_url { get; set; }
        [Required]
        public List<string> payment_options { get; set; }
        [Required]
        public FlutterwaveCustomerDTO customer { get; set; }
    }

    public class FlutterwaveCustomerDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string name { get; set; }
    }

    public class FlutterwaveResponseDTO
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public FlutterwaveResponseDataDTO Data { get; set; }
    }

    public class FlutterwaveResponseDataDTO
    {
        public string Link { get; set; }
    }
}
