
using Application.DataTransferObjects.PendingScheduleChanges;

namespace Application.Abstractions;

public interface IPendingScheduleChangeService
{
    Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync();
    Task<IEnumerable<PendingScheduleChangeResponse>> GetPendingScheduleChangesAsync(Guid workerId);
    Task AcceptPendingScheduleChange(Guid id);
    Task RejectPendingScheduleChange(Guid id);
    Task WithdrawPendingScheduleChange(Guid id);
}
