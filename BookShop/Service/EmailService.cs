using BookShop.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Service
{
    public class EmailService : IEmailService
    {
        private const string templatPath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        public async Task SendTestEmails(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}} , this is the Email Subject" , userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("TestEmail") , userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public async Task SendEmailsConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}} , confirm your email id", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ConfirmPage"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public async Task SendEmailsForgetPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}} , reset your password", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgetPassword"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            //each mail when i wanna send it it's need 3 things(Subject,body,user(From)).
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };
            foreach (var ToEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(ToEmail); //here to send (mail) to all emails i wannas send to it (ToEmails).
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };
            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail); // here we send the email.
        }
        private string GetEmailBody(string templatName)
        {
            var body = File.ReadAllText(string.Format(templatPath, templatName));
            return body;
        }
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                        text = text.Replace(placeholder.Key, placeholder.Value);
                }
            }
            return text;
        }

    }
}
