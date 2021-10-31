using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MarkMyDoctor.Services
{
    public class EmailSender : IAppEmailSender
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
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password),
                EnableSsl = true                     
            };


             await client.SendMailAsync(new MailMessage(mailSettings.Mail, email, subject, htmlMessage) { IsBodyHtml = true });
        }

        public async Task SendEmailToStaffAsync(string name, string from, string message)   
        {
            var client = new SmtpClient(mailSettings.Host, mailSettings.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(new MailMessage(from, mailSettings.Mail, name, message));

            await client.SendMailAsync(new MailMessage(mailSettings.Mail, from, "Rendszerüzenet", "Köszönjük megkeresésed, hamarosan jelentkezünk!"));
        }
    }
}
