using MimeKit;
using MailKit.Net.Smtp;

namespace TaskManagerAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendVerificationEmail(string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Task Manager", "morenoariasjordy@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Task Manager - Email Verification";

            var baseUrl = _configuration["AppSettings:BaseUrl"];
            var verificationLink = $"{baseUrl}/api/User/verify?token={token}";

            message.Body = new TextPart("html")
            {
                Text = $"<p>Click the link below to verify your account:</p> <a href='{verificationLink}'>Verificar cuenta</a>"
            };

            string? username = _configuration["EmailSettings:Username"];
            string? password = _configuration["EmailSettings:Password"];

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(username, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
