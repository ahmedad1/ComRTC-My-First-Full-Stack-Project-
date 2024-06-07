
using DataProject.Data;
using Microsoft.EntityFrameworkCore;

namespace Project1.ApplicationStartUp
{
    public class HostedService (IServiceScopeFactory ssf): IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = ssf.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.UserConnections.ExecuteDeleteAsync();
                await context.Groups.ExecuteDeleteAsync();
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;  
        }
    }
}
