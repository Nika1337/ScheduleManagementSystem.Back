using Ardalis.Specification.EntityFrameworkCore;
using Domain.Models;
using Domain.Abstractions;

namespace Infrastructure.Data;

internal class EfRepository<T>(DomainContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T> where T : BaseModel
{
}
