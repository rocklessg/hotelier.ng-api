using System.Net.Http;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IHttpClientService
    {
        Task<string> PostRequest(string url, string requestModel, string token = null);
        Task<HttpResponseMessage> GetRequest(string url, string token = null);
    }
}
