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
    internal class GetByUserConnectionHandler : IRequestHandler<GetByUserConnection, User>
    {
        UserManager<User> userManager;

        public GetByUserConnectionHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> Handle(GetByUserConnection request, CancellationToken cancellationToken)
        {
            return await userManager.Users.FirstOrDefaultAsync(x=>x.UserConnections.Any(x=>x.ConnectionId==request.UserConnection));
          
        }
    }
}
