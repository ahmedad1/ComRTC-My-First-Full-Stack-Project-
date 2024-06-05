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
    public class GetRolesOfUserHandler : IRequestHandler<GetRolesOfUser,IList<string>>
    {
        UserManager<User> userManager;

        public GetRolesOfUserHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IList<string>> Handle(GetRolesOfUser request, CancellationToken cancellationToken)
        {

            return (await userManager.GetRolesAsync(request.user));
        }
    }
}
