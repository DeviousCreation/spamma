using System.Linq.Expressions;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = this.ToExpression().Compile();
            return predicate(entity);
        }
    }
}