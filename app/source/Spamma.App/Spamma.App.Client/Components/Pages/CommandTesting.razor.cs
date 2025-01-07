using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Client.Components.Pages;

public partial class CommandTesting(ICommander commander) : ComponentBase
{  
    
    private Model _model = new Model();

    private async Task Submit()
    {
        var result = await commander.Send(new CreateDomainCommand(_model.Name));
        
    }

    public class Model
    {
        public string Name { get; set; }
    }
        
}