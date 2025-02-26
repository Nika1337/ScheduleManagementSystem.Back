

using Application.DataTransferObjects.Schedules;

namespace Application.Abstractions;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesAsync(DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesForWorkerAsync(Guid workerId, DateOnly startDate, DateOnly endDate);
    Task CreateScheduleAsync(ScheduleCreateRequest request);
    Task ChangeScheduleAsync(ScheduleChangeRequest request);
    Task RequestScheduleChangeAsync(ScheduleChangeRequest request);
}
