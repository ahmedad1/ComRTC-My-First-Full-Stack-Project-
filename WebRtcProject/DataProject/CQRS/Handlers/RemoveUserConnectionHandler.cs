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
    public class RemoveUserConnectionHandler : IRequestHandler<RemoveUserConnection>
    {
        UserManager<User> userManager;
        IMediator mediator;
        public RemoveUserConnectionHandler(UserManager<User> userManager,IMediator mediator)
        {
            this.userManager = userManager;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveUserConnection request, CancellationToken cancellationToken)
        {

            var userConnection = await mediator.Send(new GetConnectionIdObject(request.userConnection));
            
            if(userConnection == null) {
                return Unit.Value;
            }
            request.user.UserConnections.Remove(userConnection);
            await userManager.UpdateAsync(request.user);
            return Unit.Value;

            
        }
    }
}
