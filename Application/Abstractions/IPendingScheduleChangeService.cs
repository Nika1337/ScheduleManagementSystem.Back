
using Application.DataTransferObjects.PendingScheduleChanges;
using Application.DataTransferObjects.Schedules;

namespace Application.Abstractions;

public interface IPendingScheduleChangeService
{
    Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync();
    Task RequestScheduleChangeAsync(ScheduleChangeRequest request);
    Task AcceptPendingScheduleChange(Guid id);
    Task RejectPendingScheduleChange(Guid id);
    Task WithdrawPendingScheduleChange(Guid id);
}
