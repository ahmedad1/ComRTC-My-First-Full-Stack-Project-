using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class GetUsersOfGroupHandler : IRequestHandler<GetUsersOfGroup, List<object>>
    {
        AppDbContext context;

        public GetUsersOfGroupHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<object>> Handle(GetUsersOfGroup request, CancellationToken cancellationToken)
        {
            var users = context.Users.Where(x => x.Groups.Any(e => e.GroupName == request.groupName)).Select(x=>(object)new {UserName= x.UserName ,FullName=x.FullName}).ToList();
            return await Task.FromResult(users);
        }
    }
}
