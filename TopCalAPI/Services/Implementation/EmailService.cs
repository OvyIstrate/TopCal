using System;
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
        private readonly string _userName;
        private readonly string _key;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userName = Environment.GetEnvironmentVariable(_appSettings.EmailConfig.Username);
            _key = Environment.GetEnvironmentVariable(_appSettings.EmailConfig.Password);
            _client = GetSmtpClient();
        }

        public async Task SendMailAsync(EmailInfo model)
        {
            var bodyMessage =
                $"An account has been created for you. Username: {model.Username}, Password: {model.TempPassword}. Please consider changing the password once you've logged in.";

            var message = new MailMessage
            {
                To = { new MailAddress(model.UserEmail)},
                From = new MailAddress(_userName),
                Body = bodyMessage,
                Subject = "TopCal Account - Test"
            };

            await _client.SendMailAsync(message);
        }

        private SmtpClient GetSmtpClient()
        {

            return new SmtpClient(_appSettings.EmailConfig.Host, _appSettings.EmailConfig.Port)
            {
                Credentials = new NetworkCredential(_userName, _key),
                EnableSsl = true
            };
        }
    }
}
