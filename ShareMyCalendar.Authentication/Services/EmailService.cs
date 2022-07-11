using SendGrid;
using SendGrid.Helpers.Mail;
using ShareMyCalendar.Authentication.Data;

namespace ShareMyCalendar.Authentication.Services
{
    public interface IEmailService
    {
        Task SendConfirmEmail(User user);
        Task SendForgotPasswordEmail(User user);
    }

    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public Task SendConfirmEmail(User user)
        {
            var from = new EmailAddress("thomas.mathers.pro@gmail.com", "Thomas Mathers");
            var to = new EmailAddress(user.Email, user.UserName);
            var subject = "Confirm your email";
            var plainTextContent = "Confirm your email by clicking on the link below";
            var htmlContent = "Confirm your email by clicking on the link below";
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return _sendGridClient.SendEmailAsync(message);
        }

        public Task SendForgotPasswordEmail(User user)
        {
            var from = new EmailAddress("thomas.mathers.pro@gmail.com", "Thomas Mathers");
            var to = new EmailAddress(user.Email, user.UserName);
            var subject = "Reset your password";
            var plainTextContent = "Reset your password by clicking on the link below";
            var htmlContent = "Reset your password by clicking on the link below";
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return _sendGridClient.SendEmailAsync(message);
        }
    }
}
