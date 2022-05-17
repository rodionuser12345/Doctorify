using System.Linq.Expressions;

namespace Doctorify.Domain.Utils.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDesc { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}