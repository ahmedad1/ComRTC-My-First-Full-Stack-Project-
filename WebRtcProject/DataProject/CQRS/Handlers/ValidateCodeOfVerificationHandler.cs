using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using DataProject.Data.Dto;
using DataProject.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DataProject.CQRS.Handlers
{
    public class ValidateCodeOfVerificationHandler : IRequestHandler<ValidateCodeOfVerification,JWT_RefreshToken>
    {
        private readonly UserManager<User> userManager;
        private AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;    

        public ValidateCodeOfVerificationHandler(UserManager<User>userManager,IConfiguration configuration,AppDbContext context,IMediator mediator)
        {
            this.userManager = userManager;
            this.configuration=configuration;
            this.context=context;
            this.mediator=mediator;
            
        }
        public async Task<JWT_RefreshToken> Handle(ValidateCodeOfVerification request, CancellationToken cancellationToken)
        {
            var user = await mediator.Send(new GetUserByEmail(request.email));
            if (user == null)
            {
                return new JWT_RefreshToken
                {
                    EmailConfirmed=false,
                  
                };
            }
            //var userVerificationCode = (await userManager.Users.Include(x=>x.VerificationCode).FirstOrDefaultAsync(x=>x.Id==user.Id)).VerificationCode;
            var userVerificationCode = await mediator.Send(new GetVerificationCodeOfUser(user));

            if (userVerificationCode!.Code != request.code.ToString())
            {
                if (userVerificationCode.IsExpired)
                {
                    context.Verifications.Remove(userVerificationCode);
                    await context.SaveChangesAsync();
                }

                return new JWT_RefreshToken
                {
                    EmailConfirmed = false,
                    Success = false
                    
                };
            }
            if(userVerificationCode.IsExpired)
            {
                context.Verifications.Remove(userVerificationCode);
                return new JWT_RefreshToken
                {
                    EmailConfirmed = false
                   
                };
            }    
            //var role=(await mediator.Send(new GetRolesOfUser(user))).FirstOrDefault();

            var jwt = GenerateToken.GenerateJwtToken(user, configuration);
            var refreshToken = GenerateToken.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
          
            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);
           
            context.Verifications.Remove(userVerificationCode);
            await context.SaveChangesAsync();
            return new JWT_RefreshToken
            {
                Email = user.Email,
                Username = user.UserName,
                FullName = user.FullName,
                EmailConfirmed = true,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = refreshToken.Token,
                Expiration = jwt.ValidTo
            };
        }
    }
}
