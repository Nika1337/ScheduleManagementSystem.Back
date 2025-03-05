

using Application.DataTransferObjects.Schedules;

namespace Application.Abstractions;
public interface IScheduleNotificationService
{
    IAsyncEnumerable<ScheduleChangeEvent> SubscribeWorkerChanges(Guid workerId, CancellationToken cancellationToken);

    ValueTask PublishChangeAsync(ScheduleChangeEvent scheduleChangeEvent, CancellationToken cancellationToken);
}
