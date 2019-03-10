using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TopCalAPI.Config;
using TopCalAPI.Services.Interfaces;

namespace TopCalAPI.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _client;
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _client = new SmtpClient(_appSettings.EmailConfig.Host, _appSettings.EmailConfig.Port)
            {
                Credentials = new NetworkCredential(_appSettings.EmailConfig.Username, _appSettings.EmailConfig.Password),
                EnableSsl = true
            };
        }

        public async Task SendMailAsync(EmailInfo model)
        {
            var bodyMessage =
                $"An account has been created for you. Username: {model.Username}, Password: {model.TempPassword}. Please consider changing the password once you've logged in.";

            var message = new MailMessage
            {
                To = { new MailAddress(model.UserEmail)},
                From = new MailAddress(_appSettings.EmailConfig.Username),
                Body = bodyMessage,
                Subject = "TopCal Account - Test"
            };

            await _client.SendMailAsync(message);
        }
    }
}
