using MarkMyDoctor.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MarkMyDoctor.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(mailSettings.Host, mailSettings.Port)
            {
                Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password),
                EnableSsl = true
            };


           await client.SendMailAsync(new MailMessage(mailSettings.Mail, email, subject, htmlMessage) { IsBodyHtml = true });
        }
    }
}
