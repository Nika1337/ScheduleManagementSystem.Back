using Application.Abstractions;
using Application.DataTransferObjects.Schedules;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Infrastructure.Services;
internal class ScheduleNotificationService : IScheduleNotificationService
{
    private readonly ConcurrentDictionary<Guid, Channel<ScheduleChangeEvent>> _channels = new();

    private Channel<ScheduleChangeEvent> GetOrCreateChannel(Guid workerId)
    {
        return _channels.GetOrAdd(workerId, _ =>
        {
            return Channel.CreateUnbounded<ScheduleChangeEvent>(
                new UnboundedChannelOptions
                {
                    SingleReader = false,
                    SingleWriter = false
                });
        });
    }

    public IAsyncEnumerable<ScheduleChangeEvent> SubscribeWorkerChanges(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var channel = GetOrCreateChannel(userId);
        return channel.Reader.ReadAllAsync(cancellationToken);
    }

    public async ValueTask PublishChangeAsync(
        ScheduleChangeEvent ev,
        CancellationToken cancellationToken)
    {

        var channel = GetOrCreateChannel(ev.WorkerId);
        await channel.Writer.WriteAsync(ev, cancellationToken);
    }
}
