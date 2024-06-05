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
    public record IsAlreadyInDatabaseHandler : IRequestHandler<IsAlreadyInDatabase, bool>
    {
        UserManager<User> userManager;

        public IsAlreadyInDatabaseHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> Handle(IsAlreadyInDatabase request, CancellationToken cancellationToken)
        {
            return await userManager.Users.AnyAsync(x => x.Email == request.User.Email || x.UserName == request.User.UserName);
        }
    }
}
