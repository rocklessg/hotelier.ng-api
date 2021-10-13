using hotel_booking_core.Interfaces;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<HttpResponseMessage> GetRequest(string url, string token = null)
        {
            if (token != null)
            {
                HttpClientInitializer.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            using HttpResponseMessage response = await HttpClientInitializer.Client.GetAsync(url);
            return response;
        }

        public async Task<string> PostRequest(string url, string requestModel, string token = null)
        {
            if (token != null)
            {
                HttpClientInitializer.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            using (HttpResponseMessage response = await HttpClientInitializer.Client.PostAsync(url, new StringContent(requestModel, null, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                return response.ReasonPhrase;
            };
        }
    }
}
