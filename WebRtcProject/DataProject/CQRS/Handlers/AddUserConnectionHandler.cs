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
    public class AddUserConnectionHandler : IRequestHandler<AddUserConnection,User?>
    {
        UserManager<User> userManager;
        IMediator mediator;

        public AddUserConnectionHandler(UserManager<User> userManager,IMediator mediator)
        {
            this.userManager = userManager;
            this .mediator = mediator;
        }

     

       public async Task<User?> Handle(AddUserConnection request, CancellationToken cancellationToken)
        {
            var user =await mediator.Send(new GetUserByID(request.UserId)) ;
            if (user is null)
            {
                return null;
            }
            user.UserConnections.Add(new UserConnection { ConnectionId = request.UserConnection });
            await userManager.UpdateAsync(user);
            return user;
        }
    }
}
