using hotel_booking_models;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hotel_booking_utilities.EmailBodyHelper
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IUrlHelper _url;
        private readonly UserManager<AppUser> _userManager;
        public EmailBodyBuilder(IUrlHelper url, UserManager<AppUser> userManager)
        {
            _url = url;
            _userManager = userManager;
        }
        public async Task<string> GetEmailBody(AppUser user, string emailTempPath, string linkName, string token, string controllerName)
        {
            var link = string.Empty;
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(user.FirstName);

            string scheme = _url.ActionContext.HttpContext.Request.Scheme;
            var userRole = await _userManager.GetRolesAsync(user);
            foreach (var role in userRole)
            {
                if (role == UserRoles.Admin || role == UserRoles.HotelManager)
                {
                    link = _url.Action(linkName, controllerName, new { user.Email, token }, scheme);
                }
                else
                {
                    link = $"http://www.example.com/{linkName}/{token}/{user.Email}";
                }
            }

            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var newTemp = temp.Replace("**link**", link);
            var emailBody = newTemp.Replace("**User**", userName);
            return emailBody;
        }

        public async Task<string> GetEmailBody(string linkName, string emailTempPath, string token, string email, string controllerName)
        {
            string scheme = _url.ActionContext.HttpContext.Request.Scheme;
            var link = _url.Action("SendManagerInvite", "Manager", new {email, token}, scheme);
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            var emailBody = temp.Replace("**link**", link);
            return emailBody;
        }
    }
}
