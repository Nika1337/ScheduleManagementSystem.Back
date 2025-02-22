

using Application.DataTransferObjects.Schedules;

namespace Application.Abstractions;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesAsync(DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<ScheduleDetailedResponse>> GetSchedulesForWorkerAsync(Guid workerId, DateOnly firstDate, DateOnly endDate);
    Task CreateScheduleAsync(ScheduleCreateRequest request);
    Task ChangeScheduleAsync(ScheduleChangeRequest request);
    Task RequestScheduleChange(ScheduleChangeRequest request);
}
