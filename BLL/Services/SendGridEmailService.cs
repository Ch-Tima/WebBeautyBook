using Domain.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace BLL.Services
{
    /// <summary>
    /// Service for sending emails using the <see href="https://sendgrid.com/">SendGrid</see> email service.
    /// </summary>
    public class SendGridEmailService : IEmailSender
    {
        private readonly IOptions<SendGridEmailSenderOption> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridEmailService"/> class.
        /// </summary>
        /// <param name="options">The SendGrid email sender options.</param>
        public SendGridEmailService(IOptions<SendGridEmailSenderOption> options)
        {
            _options = options;
        }

        /// <summary>
        /// Sends an email asynchronously using SendGrid.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="htmlMessage">The HTML content of the email.</param>
        /// <returns>A task representing the asynchronous email sending operation.</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_options.Value.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.Value.SenderEmail, _options.Value.SenderName),
                Subject = subject,
                HtmlContent = htmlMessage,
            };

            msg.AddTo(new EmailAddress(email));
            await client.SendEmailAsync(msg);
        }
    }
}
