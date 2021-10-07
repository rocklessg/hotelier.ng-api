using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<string>> Register(RegisterUserDto userDto);
        Task<Response<LoginResponseDto>> Login(LoginDto loginDto);
        Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<Response<string>> ForgotPassword(string email);
        Task<Response<string>> ResetPassword(ResetPasswordDto model);
        Task<Response<string>> UpdatePassword(UpdatePasswordDto model);
    }
}
