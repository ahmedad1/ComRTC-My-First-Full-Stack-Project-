using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class InBlackListHandler : IRequestHandler<InBlackList, bool>
    {
         AppDbContext context;

        public InBlackListHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(InBlackList request, CancellationToken cancellationToken)
        {
            return await context.BlackListJwts.AnyAsync(x => x.TokenString == request.jwtString);
        }
    }
}
