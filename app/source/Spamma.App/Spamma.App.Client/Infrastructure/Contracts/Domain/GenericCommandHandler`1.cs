using System.Net.Http.Json;
using System.Text.Json;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public class GenericCommandHandler<TCommand>(IHttpClientFactory httpClientFactory, ILogger logger)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task<CommandResult> Handle(TCommand request, CancellationToken cancellationToken)
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
            return CommandResult.Failed(new ErrorData(ErrorCodes.CommunicationError, "Unknown API Failure"));
        }

        var resp = JsonSerializer.Deserialize<CommandResult>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        return resp ?? CommandResult.Failed(new ErrorData(ErrorCodes.CommunicationError, "Invalid JSON response"));
    }
}