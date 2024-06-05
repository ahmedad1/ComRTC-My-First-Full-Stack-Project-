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
    public class GetGroupsOfUserHandler : IRequestHandler<GetGroupsOfUser, List<string>>
    {
        UserManager<User> userManager;

        public GetGroupsOfUserHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<string>> Handle(GetGroupsOfUser request, CancellationToken cancellationToken)
        {
           var user=await userManager.FindByNameAsync(request.username);
            if (user == null)
                return null;
            var groups = user.Groups.Select(x=>x.GroupName);
            return groups.ToList();
        }
    }
}
