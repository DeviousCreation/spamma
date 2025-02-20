﻿using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public class Querier(ISender sender) : IQuerier
{
    public Task<QueryResult<TQueryResult>> Send<TQueryResult>(
        IQuery<TQueryResult> request, CancellationToken cancellationToken = default)
        where TQueryResult : IQueryResult
    {
        return sender.Send(request, cancellationToken);
    }
}