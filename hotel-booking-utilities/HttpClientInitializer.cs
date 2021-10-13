using System.Net.Http;
using System.Net.Http.Headers;

namespace hotel_booking_utilities
{
    public class HttpClientInitializer
    {
        public static HttpClient Client { get; set; }
        public static void Initialize()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
