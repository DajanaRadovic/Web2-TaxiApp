using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Interface;

namespace Common.Model
{
    public class Email : IEmail
    {
        public Task SendEmail(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.uns.ac.rs", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("radovic.pr75.2020@uns.ac.rs", "NoviSad")
            };
            var messageEmail = new MailMessage
            {
                From = new MailAddress("radovic.pr75.2020@uns.ac.rs"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            messageEmail.To.Add(email);
            return client.SendMailAsync(messageEmail);
        }
    }
}
