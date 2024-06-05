using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Handlers
{
    public record GetVerificationCodeOfUserHandler : IRequestHandler<GetVerificationCodeOfUser, Verification?>
    {
        AppDbContext context;

        public GetVerificationCodeOfUserHandler(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Verification?> Handle(GetVerificationCodeOfUser request, CancellationToken cancellationToken)
        {
            
            context.Attach(request.user);
          
            return request.user.VerificationCode;
        }
    }
}
