using System.Net.Http.Json;
using System.Text.Json;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public class GenericCommandHandler<TCommand, TResult>(IHttpClientFactory httpClientFactory, ILogger<GenericCommandHandler<TCommand, TResult>> logger)
    : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : ICommandResult
{
    public async Task<CommandResult<TResult>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("spamma");

        var response = await client.PostAsJsonAsync(
            $"api/command/{typeof(TCommand).FullName}",
            request,
            cancellationToken: cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogCritical("Command failed with non status code: {StatusCode}", response.StatusCode);
            return CommandResult<TResult>.Failed(new ErrorData(ErrorCodes.CommunicationError, "Unknown API Failure"));
        }

        var resp = JsonSerializer.Deserialize<CommandResult<TResult>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new CommandResultConverter<TResult>() },
        });

        return resp ?? CommandResult<TResult>.Failed(new ErrorData(ErrorCodes.CommunicationError, "Invalid JSON response"));
    }
}