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
    public class GetConnectionIdOfUserHandler:IRequestHandler<GetConnectionIdOfUser,string>
    {
        UserManager<User> userManager;

        public GetConnectionIdOfUserHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> Handle(GetConnectionIdOfUser request, CancellationToken cancellationToken)
        {
            return (await userManager.FindByNameAsync(request.username)).UserConnections.FirstOrDefault().ConnectionId;
        }
    }
}
