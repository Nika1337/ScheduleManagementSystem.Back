

using Application.DataTransferObjects.Workers;

namespace Application.Abstractions;

public interface IWorkerService
{
    Task CreateWorkerAsync(WorkerCreateRequest request);
    Task<WorkerProfileResponse> GetWorkerProfileAsync(Guid id);
    Task<IEnumerable<WorkerResponse>> GetWorkersAsync();
    Task UpdateWorkerAsync(WorkerProfileUpdateRequest request);
}
