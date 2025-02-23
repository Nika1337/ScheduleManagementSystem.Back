using Ardalis.Specification;

namespace Domain.Abstractions;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
