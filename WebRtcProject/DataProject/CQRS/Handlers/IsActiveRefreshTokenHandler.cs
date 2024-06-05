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
   
    public class IsActiveRefreshTokenHandler : IRequestHandler<IsActiveRefreshToken, bool>
    {
        AppDbContext context;

        public IsActiveRefreshTokenHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(IsActiveRefreshToken request, CancellationToken cancellationToken)
        {
           var token=await context.RefreshTokens.FirstOrDefaultAsync(x=>x.Token==request.refreshToken);
            if(token==null)
            {
                return false;
            }
            return token.IsActive;
        }
    }
}
