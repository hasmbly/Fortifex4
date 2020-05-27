using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailItem emailItem);
    }
}