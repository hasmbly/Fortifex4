using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUser _currentUser;

        public LoggingBehavior(ILogger<TRequest> logger, ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            string username = _currentUser.IsAuthenticated ? _currentUser.Username : "ANONYMOUS";

            _logger.LogInformation("Fortifex Request: {Name} {@Username}",
                requestName, username);

            return Task.CompletedTask;
        }
    }
}