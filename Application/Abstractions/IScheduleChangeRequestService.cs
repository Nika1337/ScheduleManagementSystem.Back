
using Application.DataTransferObjects.ScheduleChangeRequests;

namespace Application.Abstractions;

public interface IScheduleChangeRequestService
{
    Task<IEnumerable<ScheduleChangeRequestResponse>> GetScheduleChangeRequestsAsync();
    Task AcceptScheduleChangeRequest(Guid id);
    Task RejectScheduleChangeRequest(Guid id);
    Task WithdrawScheduleChangeRequest(Guid id);
}
