using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Startup;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public EmailService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> SendForgotPasswordEmailAsync(string email)
        {
            var adminAccount = await _unitOfWork.AdminRepository.GetAsync(x => x.Email.ToLower().Equals(email));
            var userAccount = await _unitOfWork.UserRepository.GetAsync(x => x.Email.ToLower().Equals(email));
            var newPassword = GenerateOTP(8);

            if (adminAccount != null)
            {
                adminAccount.Password = EncryptHelper.HashPassword(newPassword);
            }
            else if (userAccount != null)
            {
                userAccount.Password = EncryptHelper.HashPassword(newPassword);
            }
            else
            {
                return new ApiResponse().SetNotFound("Email account not found");
            }
            // Generate a new password and update the user account
            await _unitOfWork.SaveChangeAsync();
            return await SendForgotPasswordEmailAsync(email, newPassword);
        }

        public async Task<string> SendVerifyEmailAsync(string email)
        {
            var verificationCode = GenerateOTP(8);
            var message = BuildEmailTemplate(subject: "Dormy - Verify Email", to: email, content: $"Your verification code is: <b>{verificationCode}</b>");

            // Send the email
            await SendEmailAsync(message);

            return verificationCode;
        }

        private async Task<ApiResponse> SendForgotPasswordEmailAsync(string email, string newPassword)
        {
            var message = BuildEmailTemplate(subject: "Dormy - Reset Password", to: email, content: $"Your new password is: <b>{newPassword}</b>");
            // Send the email
            await SendEmailAsync(message);
            return new ApiResponse().SetOk(result: newPassword);
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

        private MimeMessage BuildEmailTemplate(string subject, string to, string content)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Dormy", "Dormy@gmail.com"));
            message.Subject = subject;
            message.To.Add(MailboxAddress.Parse(to));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =
                "<html>" +
                "<body>" +
                "<h3>Dormy</h3>" +
                "<div>" + content + "</div>" +
                "</body>" +
                "</html>"
            };
            return message;
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            string from = _configuration["EmailConfiguration:OwnerEmail"] ?? string.Empty;
            string password = _configuration["EmailConfiguration:OwnerPassword"] ?? string.Empty;
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(from, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
