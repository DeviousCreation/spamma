using System.Linq.Expressions;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    internal abstract class Specification<T>
    {
        internal abstract Expression<Func<T, bool>> ToExpression();

        internal bool IsSatisfiedBy(T entity)
        {
            var predicate = this.ToExpression().Compile();
            return predicate(entity);
        }
    }
}