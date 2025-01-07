using Spamma.App.Infrastructure.Contracts.Web;

namespace Spamma.App.Infrastructure.Web;

public class CodeGenerator(ILogger<CodeGenerator> logger) : ICodeGenerator
{
    public string? GenerateCode()
    {
        var code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        logger.LogInformation($"Generated login code: {code}");
        return code;
    }
}