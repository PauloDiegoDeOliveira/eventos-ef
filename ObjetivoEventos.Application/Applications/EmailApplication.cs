using Microsoft.Extensions.Options;
using ObjetivoEventos.Application.Dtos.SMTP;
using ObjetivoEventos.Application.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class EmailApplication : IEmailApplication
    {
        private const string templatePath = "\\wwwroot\\EmailTemplate\\{0}.html";

        private readonly SmtpOptions smtpOptions;

        public EmailApplication(IOptions<SmtpOptions> smtpOptions)
        {
            this.smtpOptions = smtpOptions.Value;
        }

        public async Task SendEmail(UserEmailOptions userEmailOptions, string emailTemplate)
        {
            userEmailOptions.Subject = UpdatePlaceHolders(userEmailOptions.Subject, userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody(emailTemplate), userEmailOptions.PlaceHolders);
            await SendEmailAsync(userEmailOptions);
        }

        private async Task SendEmailAsync(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new()
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(smtpOptions.SenderAdress, smtpOptions.SenderDisplayName),
                IsBodyHtml = smtpOptions.IsBodyHTML
            };

            foreach (string toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new(smtpOptions.UserName, smtpOptions.Password);

            SmtpClient smtpClient = new()
            {
                Host = smtpOptions.Host,
                Port = smtpOptions.Port,
                EnableSsl = smtpOptions.EnableSSL,
                UseDefaultCredentials = false,// smtpOptions.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private static string GetEmailBody(string tamplateName)
        {
            string body = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), string.Format(Directory.GetCurrentDirectory() + templatePath, tamplateName)));
            return body;
        }

        private static string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValues)
        {
            if (!string.IsNullOrEmpty(text) && keyValues != null)
            {
                foreach (KeyValuePair<string, string> placeHolder in keyValues)
                {
                    if (text.Contains(placeHolder.Key))
                    {
                        text = text.Replace(placeHolder.Key, placeHolder.Value);
                    }
                }
            }

            return text;
        }
    }
}