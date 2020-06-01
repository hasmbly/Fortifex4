using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Notifications.Models;

namespace Fortifex4.Infrastructure
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(MessageDTO message)
        {
            return Task.CompletedTask;
        }
    }
}