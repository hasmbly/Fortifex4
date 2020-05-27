using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Email;
using Fortifex4.Domain.Constants;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fortifex4.Infrastructure.Email.SendGrid
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailItem emailItem)
        {
            string apiKey = _configuration[ConfigurationKey.Email.SendGrid.APIKey];

            var client = new SendGridClient(apiKey);

            EmailAddress fromEmailAddress = new EmailAddress("fuady@fortifex.com ", "Fortifex");
            EmailAddress replyEmailAddress = new EmailAddress("fuady@fortifex.com ", "Fortifex");
            EmailAddress toEmailAddress = new EmailAddress(emailItem.ToAdress);

            var message = new SendGridMessage()
            {
                From = fromEmailAddress,
                ReplyTo = replyEmailAddress,
                Subject = emailItem.Subject,
                PlainTextContent = emailItem.Body,
                HtmlContent = emailItem.Body
            };

            message.AddTo(toEmailAddress);

            await client.SendEmailAsync(message);
        }
    }
}