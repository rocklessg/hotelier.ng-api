using hotel_booking_utilities.HttpClientService.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace hotel_booking_utilities.PaymentGatewaySettings
{
    public class FlutterwavePaymentHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;

        public FlutterwavePaymentHandler(IConfiguration configuration, IHttpClientService httpClientService)
        {
            _configuration = configuration;
            _httpClientService = httpClientService;
        }

        public async Task<FlutterwaveResponseDTO> InitializePayment(FlutterwaveRequestDTO requestDTO)
        {
            var response = await _httpClientService.PostRequest("https://api.flutterwave.com/v3/payments", JsonConvert.SerializeObject(requestDTO), _configuration["Payment:FlutterwaveKey"]);
            var paymentResponse = JsonConvert.DeserializeObject<FlutterwaveResponseDTO>(response);
            if(paymentResponse.Status == "success")
            {
                return paymentResponse;
            }
            throw new ArgumentException(paymentResponse.Message);
        }
    }

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
        public FlutterwaveResponseDataDTO Data {  get; set; }
    }

    public class FlutterwaveResponseDataDTO
    {
        public string Link {  get; set; }
    }
}
