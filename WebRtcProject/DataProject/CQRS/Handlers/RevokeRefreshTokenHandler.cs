using DataProject.CQRS.Commands;
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
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshToken>
    {
        AppDbContext context;

        public RevokeRefreshTokenHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(RevokeRefreshToken request, CancellationToken cancellationToken)
        {
            await context.RefreshTokens.Where(x => x.Token == request.RefreshToken)
                .ExecuteUpdateAsync(x=>x.SetProperty(p=>p.RevokedOn,DateTime.Now));
            return Unit.Value;

        }
    }
}
