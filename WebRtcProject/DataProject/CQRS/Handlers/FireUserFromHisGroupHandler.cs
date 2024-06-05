using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class FireUserFromHisGroupHandler : IRequestHandler<FireUserFromHisGroup,bool>
    {
        AppDbContext context;
        IMediator mediator;

        public FireUserFromHisGroupHandler(AppDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(FireUserFromHisGroup request, CancellationToken cancellationToken)
        {
           var group= request.user.Groups.FirstOrDefault();
            
            if (group == null) { return false; }
            var g = new Group()
            {
                GroupName = group.GroupName,


            };
            request.user.Groups.Remove(group);

            await context.SaveChangesAsync();
            var afterDel=await mediator.Send(new GetGroupByName(g.GroupName));
            if( afterDel.Users.Count() == 0)
            {
                context.Remove(afterDel);
                await context.SaveChangesAsync();
               
            }
            return true;

        }
    }
}
