using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace WebApp
{
    public class EmailService
    {
        public async Task SendEmail(string emailRecieve, string TextContent, string subject)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Admin", "XaJlac@gmail.com"));
            email.To.Add(new MailboxAddress("", emailRecieve));
            email.Subject = TextContent;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = subject
            };
            using (var client=new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (a, b, c, d) => true;
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("XaJlac@gmail.com", password.pass);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
    }
}
