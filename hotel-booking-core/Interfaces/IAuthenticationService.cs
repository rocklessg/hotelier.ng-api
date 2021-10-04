using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<string>> Register(RegisterUserDto userDto);
        Task<Response<LoginResponseDto>> Login(LoginDto loginDto);
        Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<Response<string>> ForgotPassword(string email);
        Task<Response<string>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<Response<bool>> UpdatePassword(RegisterUserDto addUserDto);
    }
}
