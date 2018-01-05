using CoreWeb.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CoreWeb.Models
{
    public class EmailService : IEmailService
    {
        private EmailSettings _email;

        public EmailService(IOptions<EmailSettings> email)
        {
            this._email = email.Value;
        }

        public void Send(string Subject, string Body)
        {
            using (var client = new SmtpClient(_email.SMTPServer, _email.Port))
            using (var mailMessage = new MailMessage())
            {
                if (!_email.DefaultCredentials)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_email.UserName, _email.Password);
                }

                mailMessage.From = new MailAddress(_email.From, "Email Name");
                mailMessage.To.Add(_email.To);
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = Subject;
                
                client.Send(mailMessage);
            }
        }
    }
}
