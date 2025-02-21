using Ardalis.Specification;

namespace Domain.Abstractions;

internal interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
