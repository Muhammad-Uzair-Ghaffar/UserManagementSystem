using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UserManagementSystem.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpServer = _configuration.GetSection("AppSettings:SmtpServerCreds:SmtpServer").Value;
            var smtpPort = int.Parse(_configuration.GetSection("AppSettings:SmtpServerCreds:SmtpPort").Value);
            var smtpUsername = _configuration.GetSection("AppSettings:SmtpServerCreds:SenderEmail").Value;
            var smtpPassword = _configuration.GetSection("AppSettings:SmtpServerCreds:SenderEmailPassword").Value;

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername); // Replace with your email address
                    mailMessage.To.Add(email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
