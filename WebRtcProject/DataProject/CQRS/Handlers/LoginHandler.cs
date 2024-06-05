using AutoMapper;
using Azure;
using DataProject.CQRS.Queries;
using DataProject.Data;
using DataProject.Data.Dto;
using DataProject.Tokens;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace DataProject.CQRS.Handlers
{
    public class LoginHandler : IRequestHandler<Login, JWT_RefreshToken>
    {
        private readonly UserManager<User> userManager;
        AppDbContext context;
     
        private readonly IConfiguration configuration;

        public LoginHandler(UserManager<User> userManager,AppDbContext context,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }
        public async Task<JWT_RefreshToken> Handle(Login request, CancellationToken cancellationToken)
        {
            
            var user=(await userManager.FindByNameAsync(request.loginDto.UserName));
            if (user == null||!await userManager.CheckPasswordAsync(user,request.loginDto.Password))
            {

                return new JWT_RefreshToken { Success=false};   
            }
            if(user.EmailConfirmed== false)
            {
                return new JWT_RefreshToken
                {   Email=user.Email,
                    EmailConfirmed = false,
                    Success = false
                    
                };
            }
          //  var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            
           var jwt=  GenerateToken.GenerateJwtToken(user, configuration);
           
            RefreshToken refreshToken;
            var reftoken = user.RefreshToken;

            if (reftoken == null)
            {
                refreshToken = GenerateToken.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await userManager.UpdateAsync(user);

    
            }
            else
            {
                if (!reftoken.IsActive)
                {
                    context.RefreshTokens.Remove(reftoken);
                    refreshToken = GenerateToken.GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    await userManager.UpdateAsync(user);
                }
                else
                {
                    if (reftoken.ExpiresOn <= DateTime.Now.AddHours(1))
                    {
                        context.RefreshTokens.Remove(reftoken);
                        refreshToken = GenerateToken.GenerateRefreshToken();
                        user.RefreshToken = refreshToken;
                        await userManager.UpdateAsync(user);
                    }
                    else
                    refreshToken=reftoken;

                }
            }

            return new JWT_RefreshToken
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = refreshToken.Token,
                Email = user.Email,
                FullName = user.FullName,
                Username = user.UserName,
                Expiration=jwt.ValidTo////
               
            };
        }
    }
}
