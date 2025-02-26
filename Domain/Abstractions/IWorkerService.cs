
namespace Domain.Abstractions;

public interface IWorkerService
{
    Task<(string FirstName, string LastName)> GetWorkerFullNameAsync(Guid workerId);

    Task<Dictionary<Guid, (string FirstName, string LastName)>> GetWorkerFullNamesAsync(List<Guid> ids);
}
