using System.Linq.Expressions;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    internal class ByIdSpecification<TAggregateRoot>(Guid id) : Specification<TAggregateRoot>
        where TAggregateRoot : Entity, IAggregateRoot
    {
        internal override Expression<Func<TAggregateRoot, bool>> ToExpression()
        {
            return aggregateRoot => aggregateRoot.Id == id;
        }
    }
}