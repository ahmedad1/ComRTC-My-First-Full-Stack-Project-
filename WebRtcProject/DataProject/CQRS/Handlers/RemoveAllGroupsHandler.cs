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
    public class RemoveAllGroupsHandler(AppDbContext context) : IRequestHandler<RemoveAllGroups>
    {
        public async Task<Unit> Handle(RemoveAllGroups request, CancellationToken cancellationToken)
        {
           
                await context.Groups.ExecuteDeleteAsync();
                return Unit.Value;
          
        }
    }
}
