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
    internal class GetConnectionIdObjectHandler : IRequestHandler<GetConnectionIdObject,UserConnection?>
    {
        AppDbContext context;

        public GetConnectionIdObjectHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<UserConnection?> Handle(GetConnectionIdObject request, CancellationToken cancellationToken)
        {

            return await context.UserConnections.FindAsync(request.UserConnection);
        }
    }
}
