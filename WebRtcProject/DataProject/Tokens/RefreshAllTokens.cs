using DataProject.Data;
using DataProject.Data.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DataProject.Tokens
{
    public class RefreshAllTokens : ITokens
    {
        private readonly IConfiguration configuration;
        UserManager<User> userManager;
        AppDbContext context;

        public RefreshAllTokens(UserManager<User> userManager, AppDbContext context, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<JWT_RefreshToken> UpdateTokens(string refToken)
        {

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken.Token == refToken);
            if (user == null)
                return null;
            var refreshToken = user.RefreshToken;
            if (!refreshToken.IsActive)
                return null;
            context.RefreshTokens.Remove(refreshToken);
            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var newRefreshToken = GenerateToken.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);
            var jwtToken = GenerateToken.GenerateJwtToken(user, configuration);
            return new JWT_RefreshToken
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken.Token,
                Expiration = jwtToken.ValidTo

            };



        }
    }
}
