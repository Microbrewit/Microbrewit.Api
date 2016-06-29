using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Microbrewit.Api.Service.Component
{
    public class EmailService : IEmailService
    {
        private readonly ApiSettings _settings;
        public EmailService(IOptions<ApiSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendResetPasswordMailAsync(string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Microbrew.it","microbrewit@gmail.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "Reset Password";
            message.Body = new TextPart("plain")
            {
                Text = $"Here is the password reset link: http://microbrew.it/passwordreset/{token}",
            };
            await SendMailAsync(message);
        }

        private async Task SendMailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com",587,false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("microbrewit@gmail.com",_settings.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
