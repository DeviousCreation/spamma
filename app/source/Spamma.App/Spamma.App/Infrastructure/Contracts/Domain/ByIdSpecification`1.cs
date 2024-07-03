using System.Linq.Expressions;

namespace Spamma.App.Infrastructure.Contracts.Domain
{
    public class ByIdSpecification<TAggregateRoot>(Guid id) : Specification<TAggregateRoot>
        where TAggregateRoot : Entity, IAggregateRoot
    {
        public override Expression<Func<TAggregateRoot, bool>> ToExpression()
        {
            return aggregateRoot => aggregateRoot.Id == id;
        }
    }
}