using Academy.Application.Features.WeatherForecasts.Queries;
using Academy.Shared.Notifications;
using Hangfire;
using Hangfire.Server;
using MediatR;

namespace Academy.Infrastructure.Catalog
{
    public class NumberGeneratorJob : INumberGeneratorJob
    {
        private readonly ISender _mediator;
        private readonly PerformingContext _performingContext;
        private readonly INotificationSender _notifications;
        private readonly ICurrentUser _currentUser;

        public NumberGeneratorJob(
            ISender mediator, PerformingContext performingContext,
            INotificationSender notifications, ICurrentUser currentUser
            )
        {
            _mediator = mediator;
            _performingContext = performingContext;
            _notifications = notifications;
            _currentUser = currentUser;
        }
        private async Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
        {

            await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
                _currentUser.GetUserId().ToString(),
                cancellationToken);
        }
        [Queue("notdefault")]
        //[AutomaticRetry(Attempts = 1)]
        public async Task GenerateAsync(int nSeed, CancellationToken cancellationToken)
        {
            await NotifyAsync("Your job processing has started", 0, cancellationToken);
            foreach (int index in Enumerable.Range(1, nSeed))
            {
                await Console.Out.WriteLineAsync(index.ToString());
            }
            await NotifyAsync("Job successfully completed", 0, cancellationToken);
        }
    }
}