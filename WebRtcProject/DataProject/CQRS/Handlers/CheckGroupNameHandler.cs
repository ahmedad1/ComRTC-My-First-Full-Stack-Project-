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
    public class CheckGroupNameHandler : IRequestHandler<CheckGroupName, bool>
    {
        AppDbContext context;

        public CheckGroupNameHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(CheckGroupName request, CancellationToken cancellationToken)
        {

            return await context.Groups.AnyAsync(x => x.GroupName == request.groupName);
        }
    }
}
