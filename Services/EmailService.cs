using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Services
{
    public static class EmailService
    {
        public static async Task<bool> SendPasswordResetEmail(string email, string newPassword)
        {
            try
            {
                var emailSettings = AppConfig.Configuration.GetSection("EmailSettings");

                string smtpEmail = emailSettings["SmtpEmail"];
                string smtpPassword = emailSettings["SmtpPassword"];
                string smtpHost = emailSettings["SmtpHost"];
                int smtpPort = int.Parse(emailSettings["SmtpPort"]);
                bool enableSsl = bool.Parse(emailSettings["EnableSsl"]);

                var smtp = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                    EnableSsl = enableSsl
                };

                var mail = new MailMessage()
                {
                    From = new MailAddress(smtpEmail, "QWell Support"),
                    Subject = "Password Reset",
                    Body = $"Your new password is: {newPassword}",
                    IsBodyHtml = false
                };

                mail.To.Add(email);

                await smtp.SendMailAsync(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
