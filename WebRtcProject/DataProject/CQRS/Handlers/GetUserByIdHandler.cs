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
    public record GetUserByIdHandler : IRequestHandler<GetUserByID, User?>
    {
        UserManager<User> user;

        public GetUserByIdHandler(UserManager<User> user)
        {
            this.user = user;
        }

        async Task<User?> IRequestHandler<GetUserByID, User?>.Handle(GetUserByID request, CancellationToken cancellationToken)
        {
            return await user.FindByIdAsync(request.Id);
        }
    }
}
