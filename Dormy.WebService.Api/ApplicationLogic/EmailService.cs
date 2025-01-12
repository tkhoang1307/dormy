using Dormy.WebService.Api.Core.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendVerifyEmailAsync(string email)
        {
            string from = _configuration["EmailConfiguration:OwnerEmail"] ?? string.Empty;
            string password = _configuration["EmailConfiguration:OwnerPassword"] ?? string.Empty;

            var verificationCode = GenerateOTP(8);

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Dormy", "Dormy@gmail.com"));
            message.Subject = "Dormy - Verification Email";
            message.To.Add(MailboxAddress.Parse(email));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =
                "<html>" +
                "<body>" +
                "<h3>Dormy</h3>" +
                "<div>Your verification code is: <b>" + verificationCode + "</b></div>" +
                "</body>" +
                "</html>"
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(from, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return verificationCode;
        }

        private string GenerateOTP(int length = 8)
        {
            var random = new Random();
            const string chars = "0123456789";
            StringBuilder otp = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                otp.Append(chars[random.Next(chars.Length)]);
            }
            return otp.ToString();
        }
    }
}
