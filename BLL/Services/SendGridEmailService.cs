using Domain.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace BLL.Services
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly IOptions<SendGridEmailSenderOption> _options;
        public SendGridEmailService(IOptions<SendGridEmailSenderOption> options)
        {
            _options = options;
        }

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
