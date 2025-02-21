using Ardalis.Specification;
using Domain.Models;

namespace Domain.Specification;


public abstract class BaseModelByIdSpecification<T, TResult> : SingleResultSpecification<T, TResult> where T : BaseModel
{
    protected BaseModelByIdSpecification(Guid id)
    {
        Query.Where(entity => entity.Id == id);

        Query.AsNoTracking();
    }
}