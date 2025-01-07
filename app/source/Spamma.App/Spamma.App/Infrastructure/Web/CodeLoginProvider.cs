using Spamma.App.Infrastructure.Contracts.Web;

namespace Spamma.App.Infrastructure.Web;

public class CodeLoginProvider(ICodeGenerator codeGenerator) : ICodeLoginProvider
{
    private string? _currentCode;

    public void GenerateLoginCode()
    {
        this._currentCode = codeGenerator.GenerateCode();
    }

    public bool ValidateCode(string inputCode)
    {
        return string.Equals(this._currentCode, inputCode, StringComparison.OrdinalIgnoreCase);
    }
}