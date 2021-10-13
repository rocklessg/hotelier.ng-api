using System.Net.Http;
using System.Threading.Tasks;

namespace hotel_booking_utilities.HttpClientService.Interface
{
    public interface IHttpClientService
    {
        Task<string> PostRequest(string url, string requestModel, string token = null);
        Task<HttpResponseMessage> GetRequest(string url, string token = null);
    }
}
