using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KSE.Authentication.Extensions
{
    public class SendEmail : IEmailSender
    {
        private readonly SmtpClient _smtp;
        private readonly IConfiguration _configuration;

        public SendEmail(SmtpClient smtp, IConfiguration configuration)
        {
            _smtp = smtp;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:UserName"));
            message.To.Add(email);
            message.Subject = "MyEnterprise - Confirmação de e-mail";
            message.Body = htmlMessage;
            message.IsBodyHtml = true;
            try
            {
                await _smtp.SendMailAsync(message);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }

        }
    }
}
