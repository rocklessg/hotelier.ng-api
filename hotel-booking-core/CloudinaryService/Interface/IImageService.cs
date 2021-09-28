using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace hotel_booking_core.CloudinaryService.Interface
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
        Task<DelResResult> DeleteResourcesAsync(string publicId);
    }
}
