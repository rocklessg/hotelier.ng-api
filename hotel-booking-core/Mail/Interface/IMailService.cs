using hotel_booking_models.Mail;
using System.Threading.Tasks;

namespace hotel_booking_core.Mail.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
