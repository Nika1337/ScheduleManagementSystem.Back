
using Application.DataTransferObjects.ScheduleChangeRequests;
using Application.DataTransferObjects.Schedules;

namespace Application.Abstractions;

public interface IScheduleChangeRequestService
{
    Task<IEnumerable<ScheduleChangeRequestResponse>> GetScheduleChangeRequestsAsync();
    Task RequestScheduleChangeAsync(ScheduleChangeRequest request);
    Task AcceptScheduleChangeRequest(Guid id);
    Task RejectScheduleChangeRequest(Guid id);
    Task WithdrawScheduleChangeRequest(Guid id);
}
