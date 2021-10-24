using Newtonsoft.Json;
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

    public class FlutterwaveResponseDTO<T> where T : class
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class FlutterwaveResponseDataDTO
    {
        public string Link { get; set; }
    }

    public class FlutterwaveVerifyResponseDataDTO
    {
        [JsonProperty("id")]
        public string TransactionId { get; set; }
        [JsonProperty("tx_ref")]
        public string TransactionReference { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
