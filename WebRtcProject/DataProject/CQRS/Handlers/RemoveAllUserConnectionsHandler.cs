using DataProject.CQRS.Commands;
using DataProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class RemoveAllUserConnectionsHandler(AppDbContext context) : IRequestHandler<RemoveAllUserConnections>
    {
        public async Task<Unit> Handle(RemoveAllUserConnections request, CancellationToken cancellationToken)
        {
            await context.UserConnections.ExecuteDeleteAsync();
            return Unit.Value;
        }
    }
}
