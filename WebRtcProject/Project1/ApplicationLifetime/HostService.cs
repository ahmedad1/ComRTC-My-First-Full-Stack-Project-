
using DataProject.CQRS.Commands;
using DataProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Project1.ApplicationLifetime
{
    public class HostService (IHostApplicationLifetime appLifeTime, IServiceScopeFactory factory) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            var fact = factory.CreateAsyncScope();
            using var context = fact.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.UserConnections.ExecuteDeleteAsync();
            await context.Groups.ExecuteDeleteAsync();
         
        }

        public   Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;  
        
        }
    }
}
