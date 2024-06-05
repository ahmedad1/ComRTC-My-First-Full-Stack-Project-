using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using DataProject.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    internal class AddIdentityTokenHandler(ISender sender,UserManager<User>userManager) : IRequestHandler<AddIdentityToken, string>
    {
        public async Task<string> Handle(AddIdentityToken request, CancellationToken cancellationToken)
        {
            var user=await sender.Send(new GetUserByEmail(request.email));
            if (user is null)
                return string.Empty;
            if (user.IdentityToken != null && user.IdentityToken.ExpiresAt > DateTime.Now)
                return user.IdentityToken.Token;
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);
            var token=Encoding.UTF8.GetString(tokenBytes);
            user.IdentityToken = new IdentityToken()
            {
                Token = token,
                ExpiresAt = DateTime.Now.AddMinutes(25),

            };
            await userManager.UpdateAsync(user);
            return token;
         

        }
    }
}
