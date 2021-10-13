using hotel_booking_utilities.HttpClientService.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            var response = await _httpClientService.PostRequestAsync<FlutterwaveRequestDTO, FlutterwaveResponseDTO>(
                    baseUrl: "https://api.flutterwave.com",
                    requestUrl: "v3/payments",
                    requestModel: requestDTO,
                    token: _configuration["Payment:FlutterwaveKey"]
                );
            return response;
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
        public FlutterwaveResponseDataDTO Data { get; set; }
    }

    public class FlutterwaveResponseDataDTO
    {
        public string Link { get; set; }
    }
}
