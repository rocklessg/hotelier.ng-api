using hotel_booking_dto.PaymentDtos;
using hotel_booking_utilities.HttpClientService.Interface;
using Microsoft.Extensions.Configuration;
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
}
