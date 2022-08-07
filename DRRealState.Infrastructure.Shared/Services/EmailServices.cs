using DRRealState.Core.Application.DTOS.Email;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Domain.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Shared.Services
{
    public class EmailServices : IEmailServices
    {
        private MailSettings _mailSettings { get; }

        public EmailServices(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest emailRequest)
        {

            try
            {
                BodyBuilder bodyBuilder = new()
                {
                    HtmlBody = emailRequest.Body,
                };

                MimeMessage email = new()
                {

                    Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} <{_mailSettings.EmailFrom}>"),
                    Subject = emailRequest.Subject,
                    Body = bodyBuilder.ToMessageBody()
                };
                email.To.Add(MailboxAddress.Parse(emailRequest.To));


                using SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    throw;
            }

        }
    }
}
