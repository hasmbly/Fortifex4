using System.Threading.Tasks;
using Fortifex4.Shared.Notifications.Models;

namespace Fortifex4.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(MessageDTO message);
    }
}