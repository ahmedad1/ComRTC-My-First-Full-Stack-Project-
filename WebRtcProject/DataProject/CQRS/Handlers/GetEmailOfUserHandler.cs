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
    public class GetEmailOfUserHandler : IRequestHandler<GetEmailOfUser, string>
    {
        private readonly UserManager<User> userManager;

        public GetEmailOfUserHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> Handle(GetEmailOfUser request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.username);
            return user.Email;
        }
    }
}
