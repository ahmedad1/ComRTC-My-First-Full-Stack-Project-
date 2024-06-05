using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataProject.CQRS.Handlers
{
    public class InsertVerificationCodeHandler : IRequestHandler<InsertVerifcationCode, bool>
    {
        UserManager<User> userManager;
        AppDbContext context;
        IMediator mediator;

      
        public InsertVerificationCodeHandler(UserManager<User> userManager,AppDbContext context,IMediator mediator)
        {
            this.userManager=userManager;
            this.context = context;
            this.mediator=mediator;
          
        }
        public async Task<bool> Handle(InsertVerifcationCode request, CancellationToken cancellationToken)
        {
            var user = await mediator.Send(new GetUserByEmail(request.email));
            
            if(user == null)
            {
                return false;
            }
            
            var verificationCode = await mediator.Send(new GetVerificationCodeOfUser(user));
           if(verificationCode==null)
            {
                user.VerificationCode = new Verification
                {
                    Code = request.code,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(2)

                };
               await userManager.UpdateAsync(user);
            }
           else if ( verificationCode.IsExpired)
            {
                context.Verifications.Remove(verificationCode);
                await context.SaveChangesAsync();
                user.VerificationCode=new Verification { Code=request.code, ExpiresOn=DateTime.UtcNow.AddMinutes(2) };

                await userManager.UpdateAsync(user);
                return true;
            }
            else
            {
              
                return false;
            }
            return true;
        }
    }
}
