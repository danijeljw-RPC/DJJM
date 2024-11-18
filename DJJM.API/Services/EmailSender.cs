using DJJM.API.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace DJJM.API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            
            // Validate emailSettings and required keys
            if (emailSettings == null)
            {
                throw new InvalidOperationException("Email settings are not configured.");
            }

            string senderName = emailSettings["SenderName"] ?? string.Empty;
            string senderEmail = emailSettings["SenderEmail"] ?? string.Empty;
            string smtpServer = emailSettings["SmtpServer"] ?? string.Empty;
            string portString = emailSettings["Port"] ?? string.Empty;
            string username = emailSettings["Username"] ?? string.Empty;
            string password = emailSettings["Password"] ?? string.Empty;

            if (string.IsNullOrEmpty(senderName) ||
                string.IsNullOrEmpty(senderEmail) ||
                string.IsNullOrEmpty(smtpServer) ||
                string.IsNullOrEmpty(portString) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("One or more required email settings are missing.");
            }

            if (!int.TryParse(portString, out int port))
            {
                throw new InvalidOperationException("Invalid port number in email settings.");
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
