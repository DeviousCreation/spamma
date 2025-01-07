namespace Spamma.App.Infrastructure.Emailing.Sending;

public record EmailModel(IReadOnlyList<string> BodyContent);