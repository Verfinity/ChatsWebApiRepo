using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatsWebApi.Components.Types.JWT.Logic
{
    public class JWTCreator : IJWTCreator
    {
        public string CreateJWT(User user, IAuthOptions authOptions)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.NickName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var jwt = new JwtSecurityToken(
                issuer: authOptions.Issuer,
                audience: authOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
