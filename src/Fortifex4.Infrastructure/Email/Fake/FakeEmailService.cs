using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Email;

namespace Fortifex4.Infrastructure.Email.Fake
{
    public class FakeEmailService : IEmailService
    {
        public Task SendEmailAsync(EmailItem emailItem)
        {
            return Task.CompletedTask;
        }
    }
}