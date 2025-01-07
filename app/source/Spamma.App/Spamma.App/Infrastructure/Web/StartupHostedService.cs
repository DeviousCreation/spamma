using MaintenanceModeMiddleware.Services;
using Spamma.App.Infrastructure.Contracts.Web;

namespace Spamma.App.Infrastructure.Web;

public class StartupHostedService(
    ILogger<StartupHostedService> logger,
    IMaintenanceControlService maintenanceCtrlSvc,
    ICodeLoginProvider codeLoginProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Application has started.");
        // Add your startup code here
        //maintenanceCtrlSvc.EnterMaintanence();
        
        codeLoginProvider.GenerateLoginCode();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Application is stopping.");
        return Task.CompletedTask;
    }
}