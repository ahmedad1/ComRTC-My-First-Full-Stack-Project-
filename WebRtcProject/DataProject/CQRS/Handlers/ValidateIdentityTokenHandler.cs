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
    public class ValidateIdentityTokenHandler(ISender sender,AppDbContext context) : IRequestHandler<ValidateIdentityToken, bool>
    {
        public async Task<bool> Handle(ValidateIdentityToken request, CancellationToken cancellationToken)
        {
            var user =await sender.Send(new GetUserByEmail(request.Email));
            if (user is null) return false;
            if(user.IdentityToken.Token!= request.Token || user.IdentityToken.ExpiresAt<DateTime.Now) 
                return false;
            context.Remove(user.IdentityToken);
            await context.SaveChangesAsync();
            return true;


        }
    }
}
