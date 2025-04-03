using System;
using System.Net;
using System.Net.Mail;
using CleanBrilliantCompany.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CleanBrilliantCompany.Models
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "cleanbrilliantcompanyteam5@gmail.com";
        private readonly string _smtpPass = "qzqo frce gckr crdv"; // Use App Password if using Gmail 2FA

        public void sendEmail (string toEmail, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(toEmail);
                message.From = new MailAddress(_smtpUser, "Clean Brilliant Co");
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                smtpClient.Send(message);
                Console.WriteLine($"✅ Email sent to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send email: {ex.Message}");
            }
        }
    }
}
