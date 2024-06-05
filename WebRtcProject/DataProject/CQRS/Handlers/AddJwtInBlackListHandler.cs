using DataProject.CQRS.Commands;
using DataProject.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public class AddJwtInBlackListHandler : IRequestHandler<AddJwtInBlackList, bool>
    {
        AppDbContext context;

        public AddJwtInBlackListHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(AddJwtInBlackList request, CancellationToken cancellationToken)
        {
            await context.BlackListJwts.AddAsync(new BlackListJwt { TokenString=request.token});
            await context.SaveChangesAsync();
           return true;
        }
    }
}
