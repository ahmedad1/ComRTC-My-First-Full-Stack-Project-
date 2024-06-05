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
    public class CheckOnLoadHandler : IRequestHandler<CheckOnLoad, bool>
    {
        UserManager<User> userManager;

        public CheckOnLoadHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> Handle(CheckOnLoad request, CancellationToken cancellationToken)
        {
            var user =await userManager.FindByNameAsync(request.onLoadCheckDto.Username);
            
            if(user==null|| user.Email!=request.onLoadCheckDto.Email)
                return false;
            var refreshtoken = user.RefreshToken;
            if(request.refreshToken!=refreshtoken.Token)
            {
                return false;
            }
            if(!refreshtoken.IsActive)
            {
                return false;
            }
            return true;
        }
    }
}
