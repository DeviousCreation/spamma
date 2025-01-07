namespace Spamma.App.Infrastructure.Contracts.Web;

public interface ICodeLoginProvider
{
    void GenerateLoginCode();

    public bool ValidateCode(string inputCode);
}