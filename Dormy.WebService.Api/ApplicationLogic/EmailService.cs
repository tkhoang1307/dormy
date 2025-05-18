using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Models.Enums;
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

        public async Task<bool> SendContractEmailAsync(string email, RegistrationAccommodationResponseModel registration)
        {
            var parseEnum = Enum.TryParse<ContractExtensionStatusEnum>(registration.Status, true, out var enumValue);
            var subject = "";
            var title = "";
            switch (enumValue)
            {
                case ContractExtensionStatusEnum.PENDING:
                    subject = "[Dormy] You have just created new contract.";
                    title = "You have just created new contract. Please review contract detail below: ";
                    break;
                case ContractExtensionStatusEnum.WAITING_PAYMENT:
                    subject = "[Dormy] Your accommodation registration has just been approved.";
                    title = "Your accommodation registration has just been approved. Please review contract detail below: ";
                    break;
                case ContractExtensionStatusEnum.ACTIVE:
                    subject = "[Dormy] Your contract has just been active.";
                    title = "Your contract has just been paid and active. Please review contract detail below: ";
                    break;
                case ContractExtensionStatusEnum.EXPIRED:
                    subject = "[Dormy] Your contract has just been expired.";
                    title = "Your contract has just been expired. Please review the current contract detail below: ";
                    break;
                case ContractExtensionStatusEnum.TERMINATED:
                    subject = "[Dormy] Your contract has just been terminated.";
                    title = "Your contract has just been terminated. Please review contract detail below: ";
                    break;
                case ContractExtensionStatusEnum.REJECTED:
                    subject = "[Dormy] Your accommodation registration has just been rejected.";
                    title = "Your accommodation registration has just been rejected. Please review contract detail below: ";
                    break;
            }
            var message = BuildContractEmailTemplate(subject, title, email, registration);

            // Send the email
            await SendEmailAsync(message);

            return true;
        }

        private MimeMessage BuildContractEmailTemplate(string subject, string title, string to, RegistrationAccommodationResponseModel registration)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Dormy", "Dormy@gmail.com"));
            message.Subject = subject;
            message.To.Add(MailboxAddress.Parse(to));
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine($"<h3>{title}</h3>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6;'>");

            sb.AppendLine($"<p><strong>Tenant:</strong> {registration.UserFullname}</p>");
            sb.AppendLine($"<p><strong>Submission Date:</strong> {registration.SubmissionDate:dd/MM/yyyy}</p>");
            sb.AppendLine($"<p><strong>Contract period:</strong> {registration.StartDate:dd/MM/yyyy} - {registration.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine($"<p><strong>Workplace:</strong> {registration.WorkplaceName}</p>");
            sb.AppendLine($"<p><strong>Insurance Card Number:</strong> {registration.InsuranceCardNumber}</p>");
            sb.AppendLine($"<p><strong>Registered Hospital:</strong> {registration.RegisteredHospital}</p>");
            sb.AppendLine($"<p><strong>Expiration Date:</strong> {registration.ExpirationDate:dd/MM/yyyy}</p>");
            sb.AppendLine($"<p><strong>Room Number:</strong> {registration.RoomNumber} - <strong>Building:</strong> {registration.BuildingName}</p>");
            sb.AppendLine($"<p><strong>Room Type:</strong> {registration.RoomTypeName}</p>");
            sb.AppendLine($"<p><strong>Status:</strong> {registration.Status}</p>");

            // Conditional section for extended contract
            if (registration.OrderNo != 0 && registration.ContractInformation != null)
            {
                sb.AppendLine("<hr style='margin:16px 0;'>");
                sb.AppendLine("<p><strong>BE EXTENDED FROM THE CONTRACT:</strong></p>");
                sb.AppendLine($"<p style='margin-left: 32px;'><strong>Submission Date:</strong> {registration.ContractInformation.SubmissionDate:dd/MM/yyyy}</p>");
                sb.AppendLine($"<p style='margin-left: 32px;'><strong>Contract period:</strong> {registration.ContractInformation.StartDate:dd/MM/yyyy} - {registration.ContractInformation.EndDate:dd/MM/yyyy}</p>");
                sb.AppendLine($"<p style='margin-left: 32px;'><strong>Status:</strong> {registration.ContractInformation.Status}</p>");
            }

            sb.AppendLine("</div>");
            if (registration.Status == ContractExtensionStatusEnum.WAITING_PAYMENT.ToString())
            {
                sb.AppendLine($"<h3 style='color: red;'>Please access website to pay for your contract.</h3>");
            }
            if (registration.Status == ContractExtensionStatusEnum.EXPIRED.ToString())
            {
                sb.AppendLine($"<h3 style='color: red;'>Please access website to extend or create new contract.</h3>");
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = sb.ToString()
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
