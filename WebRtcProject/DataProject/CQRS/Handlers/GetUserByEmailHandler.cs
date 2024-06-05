using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DataProject.CQRS.Handlers
{
    public record GetUserByEmailHandler : IRequestHandler<GetUserByEmail, User?>
    {  
        UserManager<User> userManager;

        public GetUserByEmailHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User?> Handle(GetUserByEmail request, CancellationToken cancellationToken)
        {
            return await userManager.FindByEmailAsync(request.Email);
        }
    }
}
