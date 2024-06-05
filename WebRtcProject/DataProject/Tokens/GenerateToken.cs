using DataProject.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataProject.Tokens
{
    public class GenerateToken
    {

        public static JwtSecurityToken GenerateJwtToken(User user, IConfiguration configuration)
        {


            var tclaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub,user.FullName),
                new Claim("EmailConfirmed",user.EmailConfirmed.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Role, "user")
            };
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken
            (
                claims: tclaims,
                audience: configuration["JWT:Audience"],
                signingCredentials: signingCredentials,
                issuer: configuration["JWT:Issuer"],
                expires: DateTime.Now.AddMinutes(60)


            );

            return jwtToken;
        }
        public static RefreshToken GenerateRefreshToken()
        {
            byte[] arr;
            arr = RandomNumberGenerator.GetBytes(32);
            return new RefreshToken
            {

                Token = Convert.ToBase64String(arr),
                CreatedOn = DateTime.UtcNow.ToLocalTime(),
                ExpiresOn = DateTime.UtcNow.ToLocalTime().AddHours(6)
            };

        }




    }
}


