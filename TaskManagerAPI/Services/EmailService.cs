using MimeKit;
using MailKit.Net.Smtp;

namespace TaskManagerAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _baseUrl;
        private readonly string _username;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? throw new ArgumentNullException("BaseUrl is not configured");
            _username = configuration["EmailSettings:Username"] ?? throw new ArgumentNullException("Username is not configured");
            _password = configuration["EmailSettings:Password"] ?? throw new ArgumentNullException("Password is not configured");
        }

        public void SendVerificationEmail(string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Task Manager", "morenoariasjordy@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Task Manager - Email Verification";

            var verificationLink = $"{_baseUrl}/api/User/verify?token={token}";

            message.Body = new TextPart("html")
            {
                Text = $"<p>Click the link below to verify your account:</p> <a href='{verificationLink}'>Verificar cuenta</a>"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(_username, _password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
