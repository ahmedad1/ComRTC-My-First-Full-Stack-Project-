using DataProject.CQRS.Queries;
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
    public record GetGroupByNameHandler : IRequestHandler<GetGroupByName, Group?>
    {
        AppDbContext context;

        public GetGroupByNameHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Group?> Handle(GetGroupByName request, CancellationToken cancellationToken)
        {
            return await context.Groups.FirstOrDefaultAsync(x=>x.GroupName==request.GroupName);
        }
    }

}
