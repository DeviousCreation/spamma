using FluentEmail.Core.Models;

namespace Spamma.TestHarnesses.EmailSender;

public record Email
{
    public List<Address> To { get; init; } = [];

    public List<Address> Cc { get; init; } = [];

    public List<Address> Bcc { get; init; } = [];

    public KeyValuePair<string, string> From { get; init; } = new(string.Empty, string.Empty);

    public string Subject { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;
}