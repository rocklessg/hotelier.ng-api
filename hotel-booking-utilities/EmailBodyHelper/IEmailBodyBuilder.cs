using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_models;

namespace hotel_booking_utilities.EmailBodyHelper
{
    public interface IEmailBodyBuilder
    {
        Task<string> GetEmailBody(AppUser user, string emailTempPath, string linkName, string token, string controllerName);
        Task<string> GetEmailBody(string linkName, string emailTempPath, string token, string email, string controllerName);
    }
}
