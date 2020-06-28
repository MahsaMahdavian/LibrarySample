using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LibraryWebSite.Services.Identity
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var Client = new SmtpClient())
            {
                var Credential = new NetworkCredential
                {
                    UserName = "Mahsaa.mahdaviaan",
                    Password = "mah0264091",
                    
                };

                Client.Credentials = Credential;
                Client.Host = "smtp.gmail.com";
                Client.Port = 587;
                Client.EnableSsl = true;
                Client.UseDefaultCredentials = false;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress("Mahsaa.mahdaviaan@gmail.com");
                    emailMessage.Subject = subject;
                    emailMessage.IsBodyHtml = true;
                    emailMessage.Body = htmlMessage;

                    Client.Send(emailMessage);
                };

                await Task.CompletedTask;
            }
        }
    }
}
