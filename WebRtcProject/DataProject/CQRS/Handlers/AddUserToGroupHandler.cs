using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class AddUserToGroupHandler : IRequestHandler<AddUserToGroup>
    {
        AppDbContext context;
        IMediator mediator;

        public AddUserToGroupHandler(AppDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(AddUserToGroup request, CancellationToken cancellationToken)
        {
            var group = await mediator.Send(new GetGroupByName(request.GroupName));
            if (group==null)
            {
              var t= await context.Groups.AddAsync(new Group { GroupName = request.GroupName });
             await context.SaveChangesAsync();
                group =t.Entity;
                context.Attach(group);
            }
            group.Users.Add(request.user);
            await context.SaveChangesAsync();
            return Unit.Value;
        }

    }
}
