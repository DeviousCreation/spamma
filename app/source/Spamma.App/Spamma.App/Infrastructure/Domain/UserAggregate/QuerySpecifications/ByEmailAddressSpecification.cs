using System.Linq.Expressions;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.QuerySpecifications
{
    internal class ByEmailAddressSpecification(string emailAddress) : Specification<User>
    {
        internal override Expression<Func<User, bool>> ToExpression()
        {
            return aggregateRoot => aggregateRoot.EmailAddress == emailAddress;
        }
    }
}